using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZID.Automat.Api.Models;
using Microsoft.Net.Http.Headers;

namespace ZID.Automat.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<object> OnPostAsync()
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "asd"),
            new Claim("FullName", "asd"),
            new Claim(ClaimTypes.Role, "Administrator"),
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return "super";
        }

        [HttpGet("Logout")]
        public async Task<object> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return "ja lol ey";
        }
    }
}