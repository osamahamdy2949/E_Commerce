using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    internal class TokenServices : ITokenServices
    {
        private readonly JwtSettings _jwtOptions;

        public TokenServices(IOptions<JwtSettings> options)
        {
            _jwtOptions = options.Value;
        }
        public string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {

            //Claims
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Name, userName)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));


            //signingCredentials [key,algo]
            var secretKey = _jwtOptions.SecretKey;

            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JWT Secret Key Is Missing");
            if(secretKey.Length < 32)
                throw new InvalidOperationException("JWT Secret Key Is Too Short");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
                signingCredentials: credintials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public double ExpirationMinutes { get; set; } = default!;
    }
}
