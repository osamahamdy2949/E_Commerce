using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IAuthenticationServices
    {
        //Login
        Task<Result<UserDto>> LoginAsync(LoginDto login, CancellationToken ct = default);
        Task<Result<UserDto>> RegisterAsync(RegisterDto register, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default);
    }
}
