using JSHOP.BLL.Mapping;
using JSHOP.PL.Extensions;
using Microsoft.Extensions.Options;

namespace JSHOP.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            MapsterConfig.RegisterMappings();
            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            builder.Services.AddApplicationServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.UseHttpsRedirection();

            app.UseRequestLocalization(
                app.Services
                    .GetRequiredService<IOptions<RequestLocalizationOptions>>()
                    .Value);

            app.UseAuthentication();
            app.UseAuthorization();

            await app.SeedDataBaseAsync();

            app.MapControllers();

            app.Run();
        }
    }
}