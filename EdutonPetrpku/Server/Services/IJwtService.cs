using EdutonPetrpku.Shared;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EdutonPetrpku.Server.Services
{
    public interface IJwtService
    {
        Task<string> CreateToken(AppUser appUser, SymmetricSecurityKey key);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, SymmetricSecurityKey key);
    }
}
