using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Services
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<AppUser> _userManager;

        public JwtService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> CreateToken(AppUser appUser, SymmetricSecurityKey key)
        {
            var roles = await _userManager.GetRolesAsync(appUser);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, appUser.Id.ToString()),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.GivenName, appUser.DisplayName),
                new Claim(ClaimTypes.Webpage, appUser.Image)

            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, SymmetricSecurityKey key)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null )
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
