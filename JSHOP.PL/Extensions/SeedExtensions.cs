using JSHOP.PL.Utils;

namespace JSHOP.PL.Extensions
{
    public static class SeedExtensions
    {
        public static async Task SeedDataBaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<ISeedData>();
                foreach (var seeder in seeders)
                {
                    await seeder.DataSeed();
                }
            }
        }
    }
}
