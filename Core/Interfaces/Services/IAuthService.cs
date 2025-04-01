using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos.User;

namespace Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto registerDto);
        // Task<string> Login(LoginDto loginDto);
        Task<AuthTokenDto> Login(LoginDto loginDto);
    }
}