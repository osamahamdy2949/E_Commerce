using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityServices(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Result<bool>.Fail(Error.NotFound("User Not Found", $"User With Email {email} Is Not Found"));
            }
            else
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }


        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto register, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                Email = register.Email,
                UserName = register.UserName,
                DisplayName = register.DispalyName,
                PhoneNumber = register.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, register.Password);

            if(!result.Succeeded)
            {
                var erorrs = result.Errors.Select(e=> new Error(e.Code,e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(erorrs);
            }

            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName,user.Email,user.UserName));
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Result<IdentityUserResult>.Fail(Error.NotFound("User Not Found", $"User With Email {email} Is Not Found"));
            }
            else
            {
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
            }
        }

        public async Task<Result<AddressDto>> GetUserAddressByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);

            if (user?.Address == null)
                return Result<AddressDto>.Fail(Error.NotFound("Address Not Found", $"Address Of User With Email {email} Is Not Exists"));

            var address = user.Address;

            return new AddressDto()
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Street = address.Street,
                City = address.City,
                Country = address.Country
            };
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Error.NotFound("User Not Found", $"User With Email {email} Not Found");

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();

        }

        public async Task<Result<bool>> IsEmailExistsAsync(string email, CancellationToken ct = default)
            => await _userManager.FindByEmailAsync(email) is not null;

        public async Task<Result<AddressDto>> UpdateOrInsertUserAddressAsync(string email, AddressDto address, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);

            if (user?.Address == null)
            {
                user.Address = new Address()
                {
                    FirstName = address.FirstName,
                    LastName = address.LastName,
                    Street = address.Street,
                    City = address.City,
                    Country = address.Country
                };
            }
            else
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return address;
            }
            else
            {
                return Error.Failure("Failure",string.Join("-",result.Errors.Select(e=>e.Description)));
            }
        }
    }
}
