using JSHOP.BLL.Common.Email;
using JSHOP.BLL.Services;
using JSHOP.BLL.Services.classes;
using JSHOP.BLL.Services.Classes;
using JSHOP.BLL.Services.Classes;
using JSHOP.BLL.Services.Interfaces;
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

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService,ProductService>();

            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}