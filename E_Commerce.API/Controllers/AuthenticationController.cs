using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationServices _authenticationServices;

        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login , CancellationToken ct)
            => ToActionResult(await _authenticationServices.LoginAsync(login, ct));
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register, CancellationToken ct)
            => ToActionResult(await _authenticationServices.RegisterAsync(register, ct));

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> EmailExists([FromQuery]string email, CancellationToken ct)
            => ToActionResult(await _authenticationServices.CheckEmailExistsAsync(email, ct));
    }
}
