using Blazored.LocalStorage;
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
        private readonly RefreshTokenService _refreshTokenService;
        private readonly ILocalStorageService _localStorage;

        public HttpInterceptorService(
            HttpClientInterceptor interceptor,
            RefreshTokenService refreshTokenService,
            ILocalStorageService localStorageService)
        {
            _interceptor = interceptor;
            _refreshTokenService = refreshTokenService;
            _localStorage = localStorageService;
        }
        public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
        public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (!absPath.Contains("token") && !absPath.Contains("accounts"))
            {
                if (await _refreshTokenService.TryRefreshToken())
                {
                    var token = await _localStorage.GetItemAsync<string>("authToken");
                    e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
            }
        }
        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}
