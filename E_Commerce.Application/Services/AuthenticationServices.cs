using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class AuthenticationServices : IAuthenticationServices
    {
        private readonly IIdentityServices _identityServices;

        public AuthenticationServices(IIdentityServices identityServices)
        {
            _identityServices = identityServices;
        }
        public async Task<Result<UserDto>> LoginAsync(LoginDto login, CancellationToken ct = default)
        {
            var userResult = await _identityServices.FindUserByEmailAsync(login.Email, ct);

            if(!userResult.IsSuccess)
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }

            var passwordResult = await _identityServices.CheckPasswordAsync(login.Email, login.Password, ct);

            if(!passwordResult.IsSuccess)
            {
                return Result<UserDto>.Fail(passwordResult.Errors);
            }
            if(!passwordResult.Data)
            {
                return Result<UserDto>.Fail(Error.UnAuthorized("Invalid Email Or Password"));
            }

            return new UserDto()
            {
                Email = login.Email,
                DisplayName = userResult.Data.DisplayName,
                Token = "Token"
            };
        }
    }
}
