using Core.Dtos.User;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Interfaces.Services;
using Core.Interfaces.Factories;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthFactory authFactory) : ControllerBase
    {
        private readonly IAuthFactory _authFactory = authFactory;

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
            return Ok(new { message = result });
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