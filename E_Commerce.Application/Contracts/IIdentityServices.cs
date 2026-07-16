using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IIdentityServices
    {
        Task<Result<IdentityUserResult>> FindUserByEmailAsync(string  email, CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto register, CancellationToken ct = default);
        Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default);
        Task<Result<bool>> IsEmailExistsAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> GetUserAddressByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateOrInsertUserAddressAsync(string email, AddressDto address, CancellationToken ct = default);
    }
}
