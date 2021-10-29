using Blazored.LocalStorage;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace EdutonPetrpku.Client.Services
{
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly AuthenticationStateProvider _authProvider;
        private readonly IAuthService _authService;

        public HttpInterceptorService(
            HttpClientInterceptor interceptor,
            AuthenticationStateProvider authProvider,
            IAuthService authService)
        {
            _interceptor = interceptor;
            _authProvider = authProvider;
            _authService = authService;
        }

        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            var url = !absPath.Contains("summary") && !absPath.Contains("refresh") && !absPath.Contains("all") && !absPath.Contains("login");
            if (url)
            {
                var ifTokenIsExpired = await TokenIsExpired();
                if(ifTokenIsExpired)
                {
                    var refreshToken = await _authService.RefreshToken();
                    if (refreshToken.Successful)
                        e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", refreshToken.Token);
                }
            }
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;

        private async Task<bool> TokenIsExpired()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"));
            if (exp is not null)
            {
                var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp.Value));
                var timeUTC = DateTime.UtcNow;
                var diff = expTime - timeUTC;
                if (diff.TotalMinutes <= 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
