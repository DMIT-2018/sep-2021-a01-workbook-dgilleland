using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                var env = services.GetRequiredService<IWebHostEnvironment>();
                if(env is not null && env.IsDevelopment())
                {
                    try
                    {
                        var configuration = services.GetRequiredService<IConfiguration>();
                        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                        string superUser = configuration.GetValue<string>("Setup:SuperUser");
                        string superMail = configuration.GetValue<string>("Setup:SuperUserEmail");
                        string superPwrd = configuration.GetValue<string>("Setup:SuperUserPassword");
                        var user = new IdentityUser
                        {
                            UserName = superUser,
                            Email = superMail,
                            EmailConfirmed = true,
                        };
                        var result = await userManager.CreateAsync(user, superPwrd);
                        if (!result.Succeeded)
                        {
                            logger.LogInformation("User was not created");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "An error occurred seeding the website users");
                        throw;
                    }
                }
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
