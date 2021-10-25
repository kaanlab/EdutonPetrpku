using Blazored.LocalStorage;
using EdutonPetrpku.Client.Providers;
using EdutonPetrpku.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EdutonPetrpku.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }


        public async Task<LoginResultViewModel> Login(LoginViewModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if (result.IsSuccessStatusCode)
            {
                var loginResult = await result.Content.ReadFromJsonAsync<LoginResultViewModel>();
                await _localStorage.SetItemAsync("authToken", loginResult.Token);
                await _localStorage.SetItemAsync("refreshToken", loginResult.RefreshToken);
                ((AppAuthStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

                return new LoginResultViewModel { Successful = true };
            }

            return new LoginResultViewModel { Successful = false, Error = "Неверное имя пользователя или пароль!" };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            ((AppAuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<RefreshTokenViewModel> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            var tokenViewModel = new TokenViewModel { Token = token, RefreshToken = refreshToken };

            var response = await _httpClient.PostAsJsonAsync("api/token/refresh", tokenViewModel);
            if (response.IsSuccessStatusCode)
            {
                var updatedTokenViewModel = await response.Content.ReadFromJsonAsync<TokenViewModel>();
                await _localStorage.SetItemAsync("authToken", updatedTokenViewModel.Token);
                await _localStorage.SetItemAsync("refreshToken", updatedTokenViewModel.RefreshToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", updatedTokenViewModel.Token);

                return new RefreshTokenViewModel { Token = updatedTokenViewModel.Token, Successful = true };
            }

            return new RefreshTokenViewModel { Successful = false };    
        }

    }
}
