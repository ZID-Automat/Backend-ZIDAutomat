using Microsoft.EntityFrameworkCore;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Application.Test
{
    public class DbSeedTest
    {
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
                return db;
            }
        }

        [Fact]
        public void TestSeed()
        {
            AutomatContext Context = this.Context;
            ISeedService seed = new SeedService(Context);
            seed.Seed();
            
            Assert.Equal(20, Context.Categories.Count());
            Assert.Equal(20, Context.Users.Count());
            Assert.Equal(100, Context.Items.Count());
            Assert.Equal(500, Context.ItemInstances.Count());
            Assert.Equal(100, Context.Admonitions.Count());
            Assert.Equal(5, Context.AdmonitionTypes.Count());
            Assert.Equal(100, Context.Borrows.Count());
        }
    }
}