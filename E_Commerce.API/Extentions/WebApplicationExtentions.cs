using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.API.Extentions
{
    public static class WebApplicationExtentions
    {
        public static async Task<WebApplication> SeedAndMigrateDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            var identitySeeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
            await seeder.SeedDataAsync();
            await identitySeeder.SeedDataAsync();

            return app;
        }
    }
}
