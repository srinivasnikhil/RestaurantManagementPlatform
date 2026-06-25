using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using System.Security.Claims;

namespace RestaurantPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _auth.RegisterAsync(dto);
            return result is null
                ? Conflict("An account with that email already exists.")
                : Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _auth.LoginAsync(dto);
            return result is null
                ? Unauthorized("Invalid email or password.")
                : Ok(result);
        }

        [Authorize]   // this endpoint requires a valid token
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            // pull the user id out of the wristband
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (idClaim is null) return Unauthorized();

            var user = await _auth.GetByIdAsync(int.Parse(idClaim));
            return user is null ? NotFound() : Ok(user);
        }
    }
}
