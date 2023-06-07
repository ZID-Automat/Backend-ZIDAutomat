using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;
using ZID.Automat.Configuration;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Exceptions;
using ZID.Automat.Repository;

namespace ZID.Automat.Application.Test
{
    public class AuthentificationTest
    {
        [Fact]
        public void TestLoginWithDebugPassword_PrevLogin()
        {
            //arange
            var JwtSecreti = "This is my JWT TEst Secret";
            var testUserConf = new TestUserCo() { TestUserName = "Username" , TestUserPassword = "Passi", UseDebug = true};
            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();

            readmock.Setup(r => r.FindByName<User>(testUserConf.TestUserName)).Returns(new User());

            var auth = new AuthentificationService(
                new JWTCo() { JWTExpireTime = 3, JWTSecret =JwtSecreti},
                testUserConf,
                new AutomatCo(),
                new AdminCo(),

                readmock.Object,
                writemock.Object
                );

            //act
            var jwt = auth.AuthUser(new UserLoginDto()
            {
                Password = testUserConf.TestUserPassword,
                Username = testUserConf.TestUserName
            });

            //assert
            writemock.Verify(m => m.Add(It.Is<User>((o) => o.Name == testUserConf.TestUserName)),Times.Never);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSecreti);
            var claims = tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);


            Assert.Contains(claims.Claims, c => c.Type == "Name" && c.Value == testUserConf.TestUserName);
            Assert.Contains(claims.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
        }
        [Fact]
        public void TestLoginWithDebugPassword_NoPrevLogin()
        {
            //arange
            var JwtSecreti = "This is my JWT TEst Secret";
            var testUserConf = new TestUserCo() { TestUserName = "Username", TestUserPassword = "Passi", UseDebug = true };
            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();


            var auth = new AuthentificationService(
                new JWTCo() { JWTExpireTime = 3, JWTSecret = JwtSecreti },
                testUserConf,
                new AutomatCo(),
                new AdminCo(),
                readmock.Object,
                writemock.Object
                );

            //act
            var jwt = auth.AuthUser(new UserLoginDto()
            {
                Password = testUserConf.TestUserPassword,
                Username = testUserConf.TestUserName
            });

            //assert
            writemock.Verify(m => m.Add(It.Is<User>((o) => o.Name == testUserConf.TestUserName)), Times.Once);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSecreti);
            var claims = tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);


            Assert.Contains(claims.Claims, c => c.Type == "Name" && c.Value == testUserConf.TestUserName);
            Assert.Contains(claims.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
        }

        [Fact]
        public void TestLoginWithDebugPassword_ButDebugOff()
        {
            //arange
            var JwtSecreti = "This is my JWT TEst Secret";
            var testUserConf = new TestUserCo() { TestUserName = "Username", TestUserPassword = "Passi", UseDebug = false };
            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();


            var auth = new AuthentificationService(
                new JWTCo() { JWTExpireTime = 3, JWTSecret = JwtSecreti },
                testUserConf,
                new AutomatCo(),
                new AdminCo(),

                readmock.Object,
                writemock.Object
                );

            //act

            Assert.ThrowsAny<PasswordWrongException>(() => auth.AuthUser(new UserLoginDto()
            {
                Password = testUserConf.TestUserPassword,
                Username = testUserConf.TestUserName
            }));

            //assert
            writemock.Verify(m => m.Add(It.Is<User>((o) => o.Name == testUserConf.TestUserName)), Times.Never);

        }

        [Fact]
        public void TestLoginWithRPassword_wrong()
        {
            //arange
            var JwtSecreti = "This is my JWT TEst Secret";
            var testUserConf = new TestUserCo() { TestUserName = "Username", TestUserPassword = "Passi", UseDebug = true };
            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();


            var auth = new AuthentificationService(
                new JWTCo() { JWTExpireTime = 3, JWTSecret = JwtSecreti },
                testUserConf,
                new AutomatCo(),
                new AdminCo(),
                readmock.Object,
                writemock.Object
                );

            //act

            Assert.ThrowsAny<PasswordWrongException>(() => auth.AuthUser(new UserLoginDto()
            {
                Password = "Test",
                Username = "asdasd"
            }));

            //assert
            writemock.Verify(m => m.Add(It.Is<User>((o) => o.Name == testUserConf.TestUserName)), Times.Never);

        }


        [Fact]
        public void TestAutomatLoginRight()
        {
            //arange

            var aut = new AutomatCo() { Password = "passiworti" };

            var JwtSecreti = "This is my JWT TEst Secret";
            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();

            var auth = new AuthentificationService(
                new JWTCo() { JWTExpireTime = 3, JWTSecret = JwtSecreti },
                new TestUserCo(),
                aut,
                new AdminCo(),
                readmock.Object,
                writemock.Object
                );

            //act
            var jwt = auth.AuthController(new ControllerLoginDto() { ControllerPassword = aut.Password });

            //assert

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSecreti);
            var claims = tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            Assert.Contains(claims.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Controller");
        }

        [Fact]
        public void TestAutomatLoginWrong()
        {
            //arange

            var aut = new AutomatCo() { Password = "passiworti" };

            var JwtSecreti = "This is my JWT TEst Secret";
            var readmock = new Mock<IRepositoryRead>();
            var writemock = new Mock<IRepositoryWrite>();

            var auth = new AuthentificationService(
                new JWTCo() { JWTExpireTime = 3, JWTSecret = JwtSecreti },
                new TestUserCo(),
                aut,
                new AdminCo(),
                readmock.Object,
                writemock.Object
                );

            //act + assert
            Assert.Throws<PasswordWrongException>(()=> auth.AuthController(new ControllerLoginDto() { ControllerPassword = "LALALA" }));
        }
    }
}