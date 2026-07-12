
using JSHOP.DAL;
using JSHOP.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace JSHOP.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext <ApplicationDbContexet>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefalutConnection"));
            });

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            var app = builder.Build();

            // Configure the HTTP request pipeline.
          

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
