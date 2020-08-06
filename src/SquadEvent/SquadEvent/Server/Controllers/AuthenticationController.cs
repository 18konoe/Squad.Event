using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SquadEvent.Shared.Models;

namespace SquadEvent.Server.Controllers
{
    public class AuthenticationController : Controller
    {
        private const string AvatarPropName = "avatar";
        private static Uri baseDiscordCdnUri = new Uri(@"https://cdn.discordapp.com/avatars/");
        private readonly IHttpClientFactory _clientFactory;
        public AuthenticationController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [AllowAnonymous]
        [HttpGet("/auth/signin")]
        public IActionResult SignIn()
        {
            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = "/",
                    IsPersistent = true,
                    // Additional set
                    AllowRefresh = true
                },
                DiscordAuthenticationDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpPost("/auth/signout")]
        public IActionResult SignOut()
        {
            return SignOut(
                new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet("/api/auth/currentuser")]
        public async Task<AuthUserInfo> GetCurrentUser()
        {
            var userInfo = new AuthUserInfo();
            if (!User.Identity.IsAuthenticated) return userInfo;
            userInfo.Name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? this.User.Identity.Name ?? string.Empty;
            userInfo.Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            userInfo.Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            userInfo.Actor = User.Claims.FirstOrDefault(c => c.Type == DiscordAuthenticationConstants.Claims.AvatarUrl)?.Value ?? string.Empty;
            //var actor = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor)?.Value ?? string.Empty;

            //var token = User.Claims.FirstOrDefault(c => c.Type == nameof(OAuthCreatingTicketContext.AccessToken))?.Value ?? string.Empty;
            //if (string.IsNullOrEmpty(token)) return userInfo;
            //var client = _clientFactory.CreateClient();
            //HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, @"https://discordapp.com/api/v6/users/@me");
            //message.Headers.Add("authorization", $"Bearer {token}");
            //var response = await client.SendAsync(message);

            //if (!response.IsSuccessStatusCode) return userInfo;
            //var jsonStream = await response.Content.ReadAsStringAsync();
            //var jsonObj = JObject.Parse(jsonStream);
            //if (!jsonObj.ContainsKey(AvatarPropName)) return userInfo;
            //var avatarId = jsonObj.Value<string>(AvatarPropName);
            //if (!string.IsNullOrEmpty(avatarId))
            //{
            //    userInfo.Actor = new Uri(baseDiscordCdnUri, $"{userInfo.Id}/{avatarId}.png");
            //}
            return userInfo;
        }
    }
}