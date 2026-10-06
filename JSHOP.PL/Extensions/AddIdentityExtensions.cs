using JSHOP.DAL.Data;
using JSHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JSHOP.PL.Extensions
{
    public  static class AddIdentityExtensions
    {
        public static IServiceCollection AddIdentityServices (this IServiceCollection services)
        {
           services.AddIdentity<ApplicationUser, IdentityRole>(
               options => options.User.RequireUniqueEmail = true
               )
               .AddEntityFrameworkStores<ApplicationDbContexet>()
               .AddDefaultTokenProviders();
            return services;
        }
    }
}
