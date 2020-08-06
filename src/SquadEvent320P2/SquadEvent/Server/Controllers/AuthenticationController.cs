using System.Linq;
using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquadEvent.Shared.Models;

namespace SquadEvent.Server.Controllers
{
    public class AuthenticationController : Controller
    {
        public AuthenticationController()
        {
        }

        [AllowAnonymous]
        [HttpGet("/auth/signin")]
        public IActionResult SignIn()
        {
            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = "/",
                    IsPersistent = true
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
        public AuthUserInfo GetCurrentUser()
        {
            var userInfo = new AuthUserInfo();
            if (User.Identity.IsAuthenticated)
            {
                userInfo.Name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? this.User.Identity.Name ?? "";
                userInfo.Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
                userInfo.Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";
            }
            return userInfo;
        }
    }
}