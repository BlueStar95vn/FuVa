using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuturifyVacation.Setup
{
    public static class RoleInitializer
    {

        public static void InitializeRoles(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = services.GetService<RoleManager<IdentityRole>>();

                if (!roleManager.RoleExistsAsync("ADMIN").Result)
                {
                    var role = roleManager.CreateAsync(new IdentityRole("ADMIN")).Result;
                }
            }
        }

    }
}
