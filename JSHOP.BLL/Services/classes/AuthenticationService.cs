
using JSHOP.BLL.Common.Email;
using JSHOP.BLL.Common.Results;
using JSHOP.BLL.Services.Interfaces;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JSHOP.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration config)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _config = config;
        }

        public async Task<Result> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var user = request.Adapt<ApplicationUser>();

                var result = await _userManager.CreateAsync(
                    user,
                    request.Password
                );

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(e => e.Description)
                    );

                    return Result.Failure(
                        new Error("REGISTRATION_FAILED", errors)
                    );
                }

                var token = await _userManager
                    .GenerateEmailConfirmationTokenAsync(user);

                token = Uri.EscapeDataString(token);

                var emailUrl =
                    $"https://localhost:7042/api/Account/confirmEmail?token={token}&userId={user.Id}";

                var emailBody = $@"
                <!DOCTYPE html>
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 40px;'>
                    <div style='max-width: 600px; margin: auto; background: white;
                                padding: 40px; border-radius: 12px; text-align: center;'>
                        <h2 style='color: #1B5E3B;'>Confirm Your Email</h2>
                        <p style='color: #555; font-size: 16px;'>
                            Thank you for creating an account with us.
                            Please confirm your email address to activate your account.
                        </p>
                        <a href='{emailUrl}'
                           style='display: inline-block; background-color: #1B5E3B;
                                  color: white; padding: 14px 30px;
                                  text-decoration: none; border-radius: 6px;
                                  font-weight: bold;'>
                            Confirm Email
                        </a>
                    </div>
                </body>
                </html>";

                await _emailSender.SendEmailAsync(
                    request.Email,
                    "Confirm Your Email",
                    emailBody
                );

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(
                    new Error(
                        "REGISTRATION_ERROR",
                        "An error occurred during registration."
                    )
                );
            }
        }

        public async Task<Result<LoginResponse>> LoginAsync(
            LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user is null)
                {
                    return Result<LoginResponse>.Failure(
                        new Error(
                            "INVALID_CREDENTIALS",
                            "Email or password is incorrect."
                        )
                    );
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Result<LoginResponse>.Failure(
                        new Error(
                            "EMAIL_NOT_CONFIRMED",
                            "Please confirm your email before logging in."
                        )
                    );
                }

                var isPasswordValid = await _userManager.CheckPasswordAsync(
                    user,
                    request.Password
                );

                if (!isPasswordValid)
                {
                    return Result<LoginResponse>.Failure(
                        new Error(
                            "INVALID_CREDENTIALS",
                            "Email or password is incorrect."
                        )
                    );
                }

                var response = new LoginResponse
                {
                    AccessToken = await GenerateJwt(user)
                };

                return Result<LoginResponse>.Success(response);
            }
            catch (Exception)
            {
                return Result<LoginResponse>.Failure(
                    new Error(
                        "LOGIN_ERROR",
                        "An error occurred during login."
                    )
                );
            }
        }

        public async Task<Result> ConfirmEmail(
            ConfirmEmailRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);

                if (user is null)
                {
                    return Result.Failure(
                        new Error(
                            "USER_NOT_FOUND",
                            "The requested user was not found."
                        )
                    );
                }

                var token = Uri.UnescapeDataString(request.Token);

                var result = await _userManager.ConfirmEmailAsync(
                    user,
                    token
                );

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(e => e.Description)
                    );

                    return Result.Failure(
                        new Error(
                            "EMAIL_CONFIRMATION_FAILED",
                            errors
                        )
                    );
                }

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(
                    new Error(
                        "EMAIL_CONFIRMATION_ERROR",
                        "An error occurred while confirming your email."
                    )
                );
            }
        }

        public async Task<string> GenerateJwt(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(
                    ClaimTypes.Name,
                    user.UserName ?? user.Email ?? user.Id
                ),
                new Claim(
                    ClaimTypes.Email,
                    user.Email ?? string.Empty
                )
            };

            claims.AddRange(
                roles.Select(role => new Claim(ClaimTypes.Role, role))
            );

            var secretKey = _config["ApiSettings:SecretKey"]
                ?? throw new InvalidOperationException(
                    "JWT secret key is not configured."
                );

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _config["ApiSettings:Issuer"],
                audience: _config["ApiSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
