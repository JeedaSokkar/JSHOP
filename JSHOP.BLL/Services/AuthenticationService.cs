using JSHOP.BLL.Common;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Services
{
   public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender  , IConfiguration config)
        {
         _userManager = userManager;
            _emailSender = emailSender;
            _config = config;
        }

     
        public async  Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user=request.Adapt<ApplicationUser>();
             var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
               

                return new RegisterResponse
                {
                    Message = "Error",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };

            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var emailUrl=$"https://localhost:7042/api/Account/confirmEmail?token={token}&userId={user.Id}";

            var emailBody = $@"
<!DOCTYPE html>
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 40px;'>

    <div style='max-width: 600px; margin: auto; background: white; 
                padding: 40px; border-radius: 12px; text-align: center;'>

        <h2 style='color: #1B5E3B;'>
            Confirm Your Email
        </h2>

        <p style='color: #555; font-size: 16px;'>
            Thank you for creating an account with us.
            Please confirm your email address to activate your account.
        </p>

        <a href='{emailUrl}'
           style='display: inline-block;
                  background-color: #1B5E3B;
                  color: white;
                  padding: 14px 30px;
                  text-decoration: none;
                  border-radius: 6px;
                  font-weight: bold;'>
            Confirm Email
        </a>
<!--
        <p style='margin-top: 30px; color: #888; font-size: 13px;'>
            If the button doesn't work, copy this link into your browser:
        </p>

        <p style='word-break: break-all; color: #1B5E3B; font-size: 12px;'>
            {emailUrl}
        </p>

    </div>
-->
</body>
</html>";

            await _emailSender.SendEmailAsync(
                request.Email,
                "Confirm Your Email",
                emailBody
            );

            return new RegisterResponse()
            {
                Message = "User registered successfully."
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
           var user=await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new LoginResponse
                {
                    Message = "Invalid email"
                };
            }
            if(! await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse
                {
                    Message = "Email not confirmed."
                };
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return new LoginResponse
                {
                    Message = "Invalid  password."
                };
            }
            return new LoginResponse
            {
                Message = "Login successful.",
                AccessToken = await GenerateJwt(user)
            };
        }

        public async Task<string> GenerateJwt (ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
       {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role,string.Join(",", roles))
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["ApiSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _config["ApiSettings:Issuer"],
                audience: _config["ApiSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> ConfirmEmail(ConfirmEmailRequest request)
        {
            var user =await _userManager.FindByIdAsync(request.UserId);
            if(user is null)
            {
                return false;
            }
            request.Token=Uri.UnescapeDataString(request.Token);

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if(!result.Succeeded)
            {
                return false;
            }
            return true;
        }
    }
}
