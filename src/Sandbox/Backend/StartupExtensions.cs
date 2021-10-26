using Backend.BLL;
using Backend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public static class StartupExtensions
    {
        public static void AddBackendDependencies(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<CapstoneContext>(options);
            services.AddTransient<AboutService>((serviceProvider) => 
            {
                var context = serviceProvider.GetRequiredService<CapstoneContext>();
                return new AboutService(context);
            });
            services.AddTransient<CapstoneService>((serviceProvider) =>
            {
                var context = serviceProvider.GetRequiredService<CapstoneContext>();
                return new CapstoneService(context);
            });
        }
    }
}
