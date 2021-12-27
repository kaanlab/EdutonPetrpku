using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
                new Claim(ClaimTypes.Webpage, appUser.Image ?? @"\upload\empty.jpg"),
                new Claim(ClaimTypes.UserData, appUser.Diploma ?? @"\upload\diploma.jpg"),
                new Claim(ClaimTypes.Uri, appUser.PdfUrl ?? @"\upload\empty.jpg")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
