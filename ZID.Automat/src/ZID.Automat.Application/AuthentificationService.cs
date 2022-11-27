using System.Security.Claims;
using System.Text;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;
using ZID.Automat.Dto.Models;
using ZID.Automat.Configuration.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ZID.Automat.Repository;
using ZID.Automat.Configuration;

namespace ZID.Automat.Application
{
    public class AuthentificationService : IUserAuthService, IControllerAuthService
    {

        private readonly JWTCo _jwtCo;
        private readonly TestUserCo _testUserCo;
        private readonly AutomatCo _automatCo;


        private readonly IUserRepository _userRepository;

        public AuthentificationService(JWTCo jwtCo, TestUserCo testUserCo, IUserRepository userRepository, AutomatCo automatCo)
        {
            _jwtCo = jwtCo;
            _testUserCo = testUserCo;
            _userRepository = userRepository;
            _automatCo = automatCo;
        }

        public string AuthUser(UserLoginDto UserLogin)
        {
            try
            {
                ADUser user = default!;
                if (!(_testUserCo.UseDebug && UserLogin.Username == _testUserCo.TestUserName && UserLogin.Password == _testUserCo.TestUserPassword))
                {
                    ADService ADServicesVar = ADService.Login(UserLogin.Username, UserLogin.Password);
                    user = ADServicesVar.CurrentUser;
                }
                else
                {
                    user = new ADUser("VornameTestUser", "NachnameTestUser", "TestUser@Spengergasse.at", _testUserCo.TestUserName, "Test", new string[] { "TestKlasse" });
                }

                if (!_userRepository.UserExists(UserLogin.Username))
                {
                    _userRepository.AddUser(new User() { Username = UserLogin.Username, Joined = DateTime.Now });
                }

                JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
                byte[] tokenKey = Encoding.ASCII.GetBytes(_jwtCo.JWTSecret);
                DateTime ExpireTime = DateTime.Now.AddHours(_jwtCo.JWTExpireTime);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("Name", user.Cn),
                        new Claim("PupilId", user.PupilId??""),
                        new Claim(ClaimTypes.Role, "User"),

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
            catch (ApplicationException)
            {
                return null!;
            }
        }

        public string AuthController(ControllerLoginDto ControllerLoginDto)
        {
            if (ControllerLoginDto.ControllerPassword != _automatCo.Password)
            {
                throw new ArgumentException("Automat Password is not valid");
            }

            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(_jwtCo.JWTSecret);
            DateTime ExpireTime = DateTime.Now.AddHours(_jwtCo.JWTExpireTime);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Role, "Controller"),

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
    }

    public interface IUserAuthService
    {
        public string AuthUser(UserLoginDto userLoginDto);
    }

    public interface IControllerAuthService
    {
        public string AuthController(ControllerLoginDto ControllerLoginDto);
    }

}