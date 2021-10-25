using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdutonPetrpku.Client.Services
{
    public class RefreshTokenService
    {
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IAuthService _authService;

        public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthService authService)
        {
            _authProvider = authProvider;
            _authService = authService;
        }

        public async Task<bool> TryRefreshToken()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"));
            if (exp is not null)
            {
                var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp.Value));
                var timeUTC = DateTime.UtcNow;
                var diff = expTime - timeUTC;
                if (diff.TotalMinutes <= 2)
                {
                    var refreshToken = await _authService.RefreshToken();
                    if (refreshToken.Successful)
                        return true;
                }
            }

            return false;
        }
    }
}
