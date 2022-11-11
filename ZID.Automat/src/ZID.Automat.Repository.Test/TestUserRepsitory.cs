using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Application;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Repository.Test
{
    public class TestUserRepsitory
    {
        private AutomatContext SeededContext
        {
            get
            {
                DbContextOptions<AutomatContext> options = new DbContextOptionsBuilder<AutomatContext>()
                .UseSqlite("Data Source=TestDb5.db")
                .Options;

                AutomatContext db = new AutomatContext(options);
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                SeedService sd = new SeedService(db);
                sd.Seed();
                return db;
            }
        }

        [Fact]
        private void TestFindUser()
        {
            var db = SeededContext;
            var UR = new UserRepository(db);

            var User = db.Users.First();

            var FU = UR.FindUser(User.Username);
            Assert.NotNull(FU);
            Assert.Equal(User.Id, FU?.Id);

            Assert.Null(UR.FindUser(""));
            Assert.Null(UR.FindUser(null!));
        }

        [Fact]
        private void TestAddUser()
        {
            var db = SeededContext;
            var UR = new UserRepository(db);

            var User = new User() { Joined = DateTime.Now, Username = "Test" };

            UR.AddUser(User);
            Assert.True(UR.UserExists(User.Username));
        }

        [Fact]
        private void TestUserExists()
        {
            var db = SeededContext;
            var UR = new UserRepository(db);

            var User = db.Users.First();

            Assert.True(UR.UserExists(User.Username));

            Assert.False(UR.UserExists(""));
            Assert.False(UR.UserExists(null!));
        }
    }
}

