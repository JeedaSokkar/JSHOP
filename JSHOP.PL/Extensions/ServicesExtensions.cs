using JSHOP.BLL.Common;
using JSHOP.BLL.Services;
using JSHOP.DAL.Repository;
using JSHOP.PL.Utils;

namespace JSHOP.PL.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddLocalizationServices();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddIdentityServices();
            services.AddJwtAuthenticationServices(configuration);

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ISeedData, RoleSeedData>();
            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}