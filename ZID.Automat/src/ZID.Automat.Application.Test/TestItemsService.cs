using Microsoft.EntityFrameworkCore;
using Xunit;
using ZID.Automat.Configuration.Model;
using ZID.Automat.Infrastructure;
using ZID.Automat.Repository;
using System.Linq;
using ZID.Automat.Domain.Models;

namespace ZID.Automat.Application.Test
{
    public class TestItemsService
    {

        private AutomatContext lastContext = default!;
        private AutomatContext Context
        {
            get
            {
                DbContextOptions<AutomatContext> options = new DbContextOptionsBuilder<AutomatContext>()
                .UseSqlite("Data Source=TestDbiiii.db")
                .Options;

                AutomatContext db = new AutomatContext(options);
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var SeedService = new SeedService(db);
                SeedService.Seed();

                lastContext = db;
                return db;
            }
        }
        

        [Fact]
        public void TestAllDisplayItems()
        {
            var context = Context;
            var item = new ItemService(new ItemRepository(context), new UserRepository(context));
            var ADI = item.AllDisplayItems();

            Assert.NotNull(ADI);
            Assert.Equal(SeedService.ITEMS, ADI.Count());
        }

        [Fact]
        public void TestPrevBorrowedDisplayItemsUser()
        {
            var context = Context;

            var u = context.Users.First();
            
            var itemS = new ItemService(new ItemRepository(context), new UserRepository(context));
            u.AddBorrow(new Borrow() { Item = new Item(), BorrowDate = DateTime.Now, PredictedReturnDate = DateTime.Now});

            context.SaveChanges();
            
            var ADI = itemS.PrevBorrowedDisplayItemsUser(u.Username);

            Assert.NotNull(ADI);
            Assert.True(ADI.Count() > 0);

            Assert.ThrowsAny<ArgumentNullException>(() => itemS.PrevBorrowedDisplayItemsUser(null!));
            Assert.ThrowsAny<ArgumentNullException>(() => itemS.PrevBorrowedDisplayItemsUser(""));
        }

        [Fact]
        public void TestTimeDetailed()
        {
            var context = Context;
            var item = context.Items.First();
            var i = new ItemService(new ItemRepository(context), new UserRepository(context));
            var Det = i.DetailedItem(item.Id);
            Assert.NotNull(Det);
        }
    }
}
