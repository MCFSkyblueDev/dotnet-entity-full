using Core.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Interfaces.Factories;
using Microsoft.AspNetCore.Antiforgery;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthFactory authFactory, IAntiforgery antiforgery) : ControllerBase
    {
        private readonly IAuthFactory _authFactory = authFactory;
        private readonly IAntiforgery _antiforgery = antiforgery;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var authService = _authFactory.CreateAuthService(registerDto.Provider);
            var result = await authService.Register(registerDto);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var authService = _authFactory.CreateAuthService(loginDto.Provider);
            var result = await authService.Login(loginDto);
            Response.Cookies.Append("access_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set true in production (HTTPS)
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(new { message = result });
            // return Ok(new { message = "Login successfully" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok(new { message = "Logged out" });
        }

        [HttpGet("csrf-token")]
        public IActionResult GetCsrfToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            });

            return Ok(new
            {
                token = tokens.RequestToken
            });
        }


        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userId);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }
            return Ok(new { message = "You are authenticated!", userId });
        }
    }

}