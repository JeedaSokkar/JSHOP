using JSHOP.BLL.Common;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Services
{
   public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender   )
        {
         _userManager = userManager;
            _emailSender = emailSender;
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
            await _emailSender.SendEmailAsync(request.Email, "Confirm Email", $"Please confirm your email by clicking this link: {emailUrl}");


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
                Message = "Login successful."
            };
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
