using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarTEDDb.DAL;
using StarTEDDb.BLL;

namespace StarTEDDb
{
    public static class StarTEDExtensions
    {
        public static void StarTEDExtensionServices(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<StarTEDContext>(options);

            services.AddTransient<ClubServices>((serviceProvider) =>
            {
                var context = serviceProvider.GetService<StarTEDContext>();
                return new ClubServices(context);
            });

			services.AddTransient<EmployeeServices>((serviceProvider) =>
			{
				var context = serviceProvider.GetService<StarTEDContext>();
				return new EmployeeServices(context);
			});
		}
    }
}
