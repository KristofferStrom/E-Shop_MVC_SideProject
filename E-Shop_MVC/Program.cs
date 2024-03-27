using E_Shop_MVC;
using E_Shop_MVC.Models.Data;
using Microsoft.AspNetCore.Identity;

namespace E_Shop_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;


                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                DataInitializer.SeedData(dbContext, userManager);


            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
