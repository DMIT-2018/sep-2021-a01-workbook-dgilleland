using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestWind.App.BLL;
using WestWind.App.DAL;

namespace WestWind.App
{
    public static class StartupExtensions
    {
        public static void AddDependencies(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<WestwindContext>(options);
            services.AddTransient<ShippingService>((provider) =>
            {
                WestwindContext dbContext = provider.GetRequiredService<WestwindContext>();
                return new ShippingService(dbContext);
            });
        }
    }
}
