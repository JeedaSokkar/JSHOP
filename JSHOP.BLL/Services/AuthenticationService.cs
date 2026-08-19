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
        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
         _userManager = userManager;
        }

     
        public async  Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user=request.Adapt<ApplicationUser>();
             var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();

                return new RegisterResponse
                {
                    Message = string.Join(", ", errors)
                };
            }
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

    }
}
