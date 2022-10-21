using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZID.Automat.Api.Models;
using Microsoft.Net.Http.Headers;
using ZID.Automat.Infrastructure;
using ZID.Automat.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace ZID.Automat.Api.Controllers
{
    /// <summary>
    /// This APIController is used to do any related Account operations
    /// </summary>
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// Login Endpoint
        /// </summary>
        /// <returns>
        /// bool which tells the User, if he is logged in
        /// </returns>
        [HttpPost("Login")]
        public async Task<bool> Login([FromBody] UserLogin UserLogin)
        {
            try
            {
                ADService ADServicesVar = ADService.Login(UserLogin.Username, UserLogin.Password);
                ADUser user = ADServicesVar.CurrentUser;
                var claims = new List<Claim>();
                claims.Add(new Claim("Username", user.Cn));
                claims.Add(new Claim("PupilId", user.PupilId??""));

                claims.Add(new Claim(ClaimTypes.Role, "User"));

               
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}