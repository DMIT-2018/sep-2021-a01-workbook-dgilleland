using Backend.BLL;
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
using WebApp.Data;

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
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                var env = services.GetRequiredService<IWebHostEnvironment>();
                if (env is not null && env.IsDevelopment())
                {
                    try
                    {
                        var configuration = services.GetRequiredService<IConfiguration>();
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        if (!userManager.Users.Any())
                        {
                            var capstoneService = services.GetRequiredService<CapstoneService>();
                            var instructors = capstoneService.ListInstructors();
                            string emailSuffix = configuration.GetValue<string>("Setup:SchoolEmail");
                            string password = configuration.GetValue<string>("Setup:InitialPassword");
                            foreach (var instructor in instructors)
                            {
                                var user = new ApplicationUser
                                {
                                    UserName = instructor.Text.Replace(" ", "."),
                                    Email = instructor.Text.Replace(" ", ".") + emailSuffix,
                                    StaffId = instructor.Value,
                                    EmailConfirmed = true
                                };
                                var result = await userManager.CreateAsync(user, password);
                                if (!result.Succeeded)
                                {
                                    logger.LogInformation("User was not created");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "An error occurred seeing the website users");
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
