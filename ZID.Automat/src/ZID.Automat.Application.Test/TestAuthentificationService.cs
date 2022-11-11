using ZID.Automat.Dto.Models;
using ZID.Automat.Configuration.Model;
using ZID.Automat.Repository;
using Microsoft.EntityFrameworkCore;
using ZID.Automat.Infrastructure;

using Xunit;

namespace ZID.Automat.Application.Test
{
    public class TestAuthentificationService
    {
        private AutomatContext lastContext = default!;
        private AutomatContext Context
        {
            get
            {
                DbContextOptions<AutomatContext> options = new DbContextOptionsBuilder<AutomatContext>()
                .UseSqlite("Data Source=TestDb.db")
                .Options;

                AutomatContext db = new AutomatContext(options);
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                lastContext = db;
                return db;
            }
        }
        private AuthentificationService AuthService => new AuthentificationService(new JWTCo() { JWTExpireTime = 1,JWTSecret = "Test Secret das ist ein Secret"},new TestUserCo() { UseDebug = true, TestUserName = "TestUserName",TestUserPassword = "TestUserPassword" },new UserRepository(Context));

        [Fact]
        public void TestuserLogin()
        {
            AuthentificationService authService = AuthService;
            string? JWT1 = authService.AuthUser(new UserLoginDto() {Username = "TestUserName", Password= "TestUserPassword"});
            Assert.NotNull(JWT1);
            Assert.Equal(1, lastContext.Users.Count());

            string? JWT3 = authService.AuthUser(new UserLoginDto() { Username = "TestUserName", Password = "TestUserPassword" });
            Assert.Equal(1, lastContext.Users.Count());


            string? JWT2 = authService.AuthUser(new UserLoginDto() { Username = "TesUserName12", Password = "TestUserPassword12" });
            Assert.Null(JWT2);
            Assert.Equal(1, lastContext.Users.Count());
        }
    }
}
