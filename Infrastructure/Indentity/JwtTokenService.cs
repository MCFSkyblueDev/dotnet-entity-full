using System.Security.Claims;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace Infrastructure.Indentity
{
    public class JwtTokenService(IOptions<AppSettings> appSettings) : IJwtTokenService
    {
        private readonly AppSettings _appSettings = appSettings.Value;
        public string GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Jwt.Key);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim> {
                // new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString());
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_appSettings.Jwt.ExpiryDays),
                SigningCredentials = credentials,
                Issuer = _appSettings.Jwt.Issuer,
                Audience = _appSettings.Jwt.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Jwt.Key);
            
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            return tokenHandler.ValidateToken(token, parameters, out _);
        }
    }
}