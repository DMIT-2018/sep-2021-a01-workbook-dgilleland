using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestWind.App.DAL;

namespace WestWind.App
{
    public static class StartupExtensions
    {
        public static void AddDependencies(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<WestwindContext>(options);
        }
    }
}
