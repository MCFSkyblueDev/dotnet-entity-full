using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.User
{
    public class AuthTokenDto
    {
        public required string Token { get; set; }
        // public required string RefreshToken { get; set; }
    }
}