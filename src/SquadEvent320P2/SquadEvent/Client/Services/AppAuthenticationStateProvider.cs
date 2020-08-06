using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SquadEvent.Shared.Models;

namespace SquadEvent.Client.Services
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient HttpClient;

        public AppAuthenticationStateProvider(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await this.HttpClient.GetJsonAsync<AuthUserInfo>("/api/auth/currentuser");
            var identity = default(ClaimsIdentity);
            if (!string.IsNullOrEmpty(user?.Name) && !string.IsNullOrEmpty(user?.Id) && !string.IsNullOrEmpty(user?.Email))
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                }, authenticationType: "Discord");
            }
            return new AuthenticationState(new ClaimsPrincipal(identity ?? new ClaimsIdentity()));
        }

        public Task<AuthenticationState> RefreshAsync()
        {
            var stateAsync = this.GetAuthenticationStateAsync();
            this.NotifyAuthenticationStateChanged(stateAsync);
            return stateAsync;
        }
    }
}
