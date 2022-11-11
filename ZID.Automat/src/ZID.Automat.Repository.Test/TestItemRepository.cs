using Microsoft.EntityFrameworkCore;
using ZID.Automat.Application;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;
using System.Linq;
namespace ZID.Automat.Repository.Test
{
    public class TestItemRepository
    {
        private AutomatContext SeededContext
        {
            get
            {
                DbContextOptions<AutomatContext> options = new DbContextOptionsBuilder<AutomatContext>()
                .UseSqlite("Data Source=TestDb.db")
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
        public void TestGetItemWithItemInstance()
        {
            AutomatContext context = SeededContext;

            ItemRepository ItemRepository = new ItemRepository(context);
            var list = ItemRepository.getItemWithItemInstance();
            foreach (var entry in list)
            {
                Assert.NotNull(entry.ItemInstances);
            }
        }

        [Fact]
        public void TestGetPrevBorrowedItemsOfUser()
        {
            AutomatContext context = SeededContext;
            ItemRepository ItemRepository = new ItemRepository(context);
            User user = context.Users.First();
            var result = ItemRepository.getPrevBorrowedItemsOfUser(user.Id);
            
            foreach(var Item in result)
            {
                Assert.NotNull(Item.ItemInstances);
                var right = Item.ItemInstances.Where(II => II.Borrows.Where(B => B.UserId == user.Id) != null).ToList();
                Assert.NotEmpty(right);
            } 
        }
    }
}



// Seed Service is in Application but I need it in the Repository test
//Code wiederholung verhindern(DbContext) in Tests
//User.Claims.First(c=>c.Issuer == "Name").Value ersetzen