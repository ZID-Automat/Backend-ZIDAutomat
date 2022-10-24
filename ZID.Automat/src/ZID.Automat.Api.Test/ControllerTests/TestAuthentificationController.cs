using ZID.Automat.Api.Controllers;
using ZID.Automat.Api.Models;

namespace ZID.Automat.Api.Test.ControllerTests
{
    public class TestAuthentificationController
    {
        public AuthentificationController AuthController => new AuthentificationController("This is a very sercret Key", 10, true, "TestUserName", "TestUserPassword");

        [Fact]
        public void TestuserLogin()
        {
            AuthentificationController authController = AuthController;
            string? JWT1 = authController.UserLogin(new UserLogin() { Username = "TestUserName", Password = "TestUserPassword" });
            Assert.NotNull(JWT1);

            string? JWT2 = authController.UserLogin(new UserLogin() { Username = "TesUserName12", Password = "TestUserPassword12" });
            Assert.Null(JWT2);
            Assert.True(false);
        }

    }
}