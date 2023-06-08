using System.Security.Claims;
using System.Text;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;
using ZID.Automat.Dto.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ZID.Automat.Repository;
using ZID.Automat.Configuration;
using ZID.Automat.Exceptions;
using System.Net;

namespace ZID.Automat.Application
{
    public class AuthentificationService : IUserAuthService, IControllerAuthService, IAdminAuthService
    {

        private readonly JWTCo _jwtCo;
        private readonly TestUserCo _testUserCo;
        private readonly AutomatCo _automatCo;
        private readonly AdminCo _adminCo;
        private readonly IRepositoryRead _readRepo;
        private readonly IRepositoryWrite _writeRepo;


        public AuthentificationService(JWTCo jwtCo, TestUserCo testUserCo, AutomatCo automatCo, AdminCo adminCo, IRepositoryRead readRepo, IRepositoryWrite writeRepo)
        {
            _jwtCo = jwtCo;
            _testUserCo = testUserCo;
            _automatCo = automatCo;
            _adminCo = adminCo;
            _readRepo = readRepo;
            _writeRepo = writeRepo;
        }

        public string AuthUser(UserLoginDto UserLogin)
        {
            ADUser user = default!;
            if (!(_testUserCo.UseDebug && UserLogin.Username == _testUserCo.TestUserName && UserLogin.Password == _testUserCo.TestUserPassword))
            {
                ADService ADServicesVar = ADService.Login(UserLogin.Username, UserLogin.Password)??throw new PasswordWrongException();
                user = ADServicesVar.CurrentUser;
            }
            else
            {
                user = new ADUser("VornameTestUser", "NachnameTestUser", "TestUser@Spengergasse.at", _testUserCo.TestUserName, "Test", new string[] { "TestKlasse" });
            }

            var userDb = _readRepo.FindByName<User>(UserLogin.Username);
            if (userDb == null)
            {
                userDb = new User() { Name = UserLogin.Username, Vorname = user.Firstname, Nachname = user.Lastname, Joined = DateTime.Now };
                _writeRepo.Add(userDb);
            }

            userDb.LastLogin = DateTime.Now;
            _writeRepo.Update(userDb);
            

            if (userDb?.Blockiert==true)
            {
                throw new UserBlockedException();
            }

            return GenJWT(
                new ClaimsIdentity(new Claim[]
                    {
                            new Claim("Name", user.Cn),
                            new Claim("PupilId", user.PupilId ?? ""),
                            new Claim(ClaimTypes.Role, "User")
                    }))??throw new PasswordWrongException();
        }

        public string AuthController(ControllerLoginDto ControllerLoginDto)
        {
            if (ControllerLoginDto.ControllerPassword != _automatCo.Password)   
            {
                throw new PasswordWrongException();
            }

            return GenJWT(new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Role, "Controller"),
                }));
        }
        
        public string AuthAdmin(AdminLoginDto adminLoginDto)
        {
            if (adminLoginDto.Hall != _adminCo.Hall)
            {
                    throw new PasswordWrongException();
            }

            return GenJWT(new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Role, "Admin"),
                }));
        }

        private string GenJWT(ClaimsIdentity claimsIdentity)
        {
            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(_jwtCo.JWTSecret);
            DateTime ExpireTime = DateTime.Now.AddHours(_jwtCo.JWTExpireTime);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
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

    public interface IAdminAuthService
    {
        public string AuthAdmin(AdminLoginDto adminLoginDto);
    }

    public interface IControllerAuthService
    {
        public string AuthController(ControllerLoginDto ControllerLoginDto);
    }
}

  
