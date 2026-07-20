using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class CatalogDataSeeder(StoreDbContext dbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        private async Task SeedIfEmpty<T, TKey>(string rootPath, string fileName, CancellationToken ct) where T :BaseEntity<TKey>
        {
            if(await dbContext.Set<T>().AnyAsync())
            {
                logger.LogInformation("Data Already Sedded");
                return;
            }

            var filePath = Path.Combine(rootPath, fileName);

            if(!File.Exists(filePath))
            {
                logger.LogWarning($"File: {fileName} is Not Found");
                return;
            }

            using var stream = File.OpenRead(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, options, ct);

            if (items?.Any() ?? false)
                dbContext.Set<T>().AddRange(items);
        }

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);

                if (pendingMigrations.Any())
                    await dbContext.Database.MigrateAsync(ct);

                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");

                await SeedIfEmpty<ProductBrand, int>(seedRoot, "brands.json", ct);
                await SeedIfEmpty<ProductType, int>(seedRoot, "types.json", ct);
                await SeedIfEmpty<Product, int>(seedRoot, "products.json", ct);
                await SeedIfEmpty<DeliveryMethod, int>(seedRoot, "delivery.json", ct);

                var result = await dbContext.SaveChangesAsync(ct);

                if (result > 0)
                    logger.LogInformation($"{result} Rows Added");
                else
                    logger.LogInformation("Database Already Seeded");

            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
            }
        }
    }
}
