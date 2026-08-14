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
        private readonly UserManager<ApplicationUser> userManager;
        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        
        
public async  Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user=request.Adapt<ApplicationUser>();
             var result = await userManager.CreateAsync(user, request.Password);
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
    }
}
