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
        private readonly ITokenServices _tokenServices;

        public AuthenticationServices(IIdentityServices identityServices, ITokenServices tokenServices)
        {
            _identityServices = identityServices;
            _tokenServices = tokenServices;
        }

        public async Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default)
            => await _identityServices.IsEmailExistsAsync(email, ct);

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

            var user = userResult.Data;
            var rolesResult = await _identityServices.GetUserRolesAsync(user.Email);
            var roles = rolesResult.Data;
            var token = _tokenServices.CreateToken(user.Id, user.Email, user.Username,roles);

            return new UserDto()
            {
                Email = login.Email,
                DisplayName = userResult.Data.DisplayName,
                Token = token
            };
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto register, CancellationToken ct = default)
        {
            var userResult = await _identityServices.CreateUserAsync(register, ct);

            if(!userResult.IsSuccess)
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }

            var user = userResult.Data;
            var rolesResult = await _identityServices.GetUserRolesAsync(user.Email);
            var roles = rolesResult.Data;
            var token = _tokenServices.CreateToken(user.Id, user.Email, user.Username, roles);

            return Result<UserDto>.Ok(new UserDto()
            {
                Email = register.Email,
                DisplayName = register.DispalyName,
                Token = token
            });
        }
    }
}
