using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _identityDb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataSeeder> _logger;

        public IdentityDataSeeder(StoreIdentityDbContext identityDb, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ILogger<IdentityDataSeeder> logger)
        {
            _identityDb = identityDb;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendindMigrations = await _identityDb.Database.GetPendingMigrationsAsync(ct);

                if (pendindMigrations.Any())
                {
                    await _identityDb.Database.MigrateAsync();
                }

                if (!await _roleManager.Roles.AnyAsync())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!await _userManager.Users.AnyAsync(ct))
                {
                    var admin = new ApplicationUser()
                    {
                        DisplayName = "Osama Hamdy",
                        Email = "osama@gmail.com",
                        UserName = "OsamaHamdy",
                        PhoneNumber = "01234567890"
                    };
                    var createResult = await _userManager.CreateAsync(admin, "P@ssw0rd");

                    if (createResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(admin, "SuperAdmin");
                    }
                    else
                    {
                        var errors = string.Join('-', createResult.Errors.Select(e => e.Description));
                        _logger.LogWarning($"Can't Seed Defualt Admin {errors}");
                    }
                }
            }
            catch
            {
                _logger.LogError("Identity Data Seeding Failed");
            }

        }
    }
}
