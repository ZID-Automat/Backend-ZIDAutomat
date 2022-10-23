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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;

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
        /// <summary>
        /// Login Endpoint
        /// </summary>
        /// <returns>
        /// bool which tells the User, if he is logged in
        /// </returns>
        string JWTSecret = "";
        int JWTExpireTime = 0;

        bool UseDebug = false;
        string TestUserName = "";
        string TestUserPassword = "";

        [ActivatorUtilitiesConstructor]

        public AuthentificationController(IConfiguration configuration)
        {
            var JWTConf = configuration.GetSection("UserLoginConf").GetSection("JWT");
            JWTSecret = JWTConf.GetValue<string>("JWTSecret");
            JWTExpireTime = JWTConf.GetValue<int>("JWTExpireTime");

            var DebugConf = configuration.GetSection("Debug");
            UseDebug = DebugConf.GetValue<bool>("useDebug");

            var TestUserConf = DebugConf.GetSection("UserAuth").GetSection("TestUser");
            TestUserName = TestUserConf.GetValue<string>("TestUserName");
            TestUserPassword = TestUserConf.GetValue<string>("TestUserPassword");
        }

        public AuthentificationController(string jWTSecret, int jWTExpireTime, bool useDebug, string testUserName, string testUserPassword)
        {
            JWTSecret = jWTSecret;
            JWTExpireTime = jWTExpireTime;
            UseDebug = useDebug;
            TestUserName = testUserName;
            TestUserPassword = testUserPassword;
        }

        [HttpPost("Login")]
        public string? UserLogin([FromBody] UserLogin UserLogin)
        {
            try
            {
                ADUser user = default!;
                if (!(UseDebug && UserLogin.Username == TestUserName && UserLogin.Password == TestUserPassword))
                {
                    ADService ADServicesVar = ADService.Login(UserLogin.Username, UserLogin.Password);
                    user = ADServicesVar.CurrentUser;
                }
                else
                {
                    user = new ADUser("VornameTestUser", "NachnameTestUser", "TestUser@Spengergasse.at", "TES99999", "Test", new string[] { "TestKlasse" });
                }

                JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
                byte[] tokenKey = Encoding.ASCII.GetBytes(JWTSecret);
                DateTime ExpireTime = DateTime.Now.AddHours(JWTExpireTime);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("Username", user.Cn),
                        new Claim("PupilId", user.PupilId??""),
                        new Claim(ClaimTypes.Role, "User")

                }),
                    Expires = ExpireTime,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                        )
                };

                SecurityToken token = Handler.CreateToken(tokenDescriptor);
                return Handler.WriteToken(token);
            }
            catch (ApplicationException e)
            {
                return null;
            }
        }
    }
}