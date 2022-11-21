using Microsoft.EntityFrameworkCore;
using ZID.Automat.Infrastructure;

using Xunit;

namespace ZID.Automat.Application.Test
{
    public class TestDbSeedService
    {
        private AutomatContext Context
        {
            get
            {
                DbContextOptions<AutomatContext> options = new DbContextOptionsBuilder<AutomatContext>()
                .UseSqlite("Data Source=TestDb2.db")
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
            
            Assert.Equal(SeedService.CATE, Context.Categories.Count());
            Assert.Equal(SeedService.USERS, Context.Users.Count());
            Assert.Equal(SeedService.ITEMS, Context.Items.Count());
            Assert.Equal(SeedService.ITEMSINSTANCE, Context.ItemInstances.Count());
            Assert.Equal(SeedService.ADMON, Context.Admonitions.Count());
            Assert.Equal(SeedService.ADMONTYPE, Context.AdmonitionTypes.Count());
            Assert.Equal(SeedService.BORROWS, Context.Borrows.Count());
        }
    }
}