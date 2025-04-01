using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserEntity user);

        // bool ValidateToken(string token);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}