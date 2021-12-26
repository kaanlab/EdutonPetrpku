using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace EdutonPetrpku.Client.Providers
{
    public class AppAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AppAuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwt(savedToken);

            if(TokenIsExpired(claims.First(c => c.Type == ClaimTypes.Expired)))
            {
                await _localStorage.RemoveItemAsync("authToken");
                MarkUserAsLoggedOut();
                _httpClient.DefaultRequestHeaders.Authorization = null;
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwtToken)
        {
            var token = new JwtSecurityToken(jwtEncodedString: jwtToken);

            yield return new Claim(ClaimTypes.Sid, token.Claims.First(c => c.Type.Contains("sid"))?.Value);
            yield return new Claim(ClaimTypes.Name, token.Claims.First(c => c.Type.Equals("unique_name"))?.Value);
            yield return new Claim(ClaimTypes.GivenName, token.Claims.First(c => c.Type.Equals("given_name"))?.Value);
            yield return new Claim(ClaimTypes.Webpage, token.Claims.First(c => c.Type.Equals("website"))?.Value);
            yield return new Claim(ClaimTypes.Role, token.Claims.First(c => c.Type.Equals("role"))?.Value);
            yield return new Claim(ClaimTypes.UserData, token.Claims.First(c => c.Type.Contains("userdata"))?.Value);
            yield return new Claim(ClaimTypes.Expired, token.Claims.First(c => c.Type.Equals("exp"))?.Value);
        }

        private bool TokenIsExpired(Claim exp)
        {
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
