using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ZID.Automat.Infrastructure;
using ZID.Automat.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using ZID.Automat.Dto.Models;
using ZID.Automat.Configuration.Model;
using ZID.Automat.Application;

namespace ZID.Automat.Api.Controllers
{
    /// <summary>
    /// This APIController is used to do any related Account operations
    /// </summary>
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IUserAuth _userAuth;
        public AuthentificationController(IUserAuth userAuth)
        {
            _userAuth = userAuth;
        }

        [HttpPost("Login")]
        public string? UserLogin([FromBody] UserLoginDto UserLogin)
        {
            return _userAuth.AuthUser(UserLogin);
        }
    }
}