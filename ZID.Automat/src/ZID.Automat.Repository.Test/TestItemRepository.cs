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
                .UseSqlite("Data Source=Test5Db.db")
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

        [Fact]
        public void TestItemAvalable()
        {
            AutomatContext context = SeededContext;
            ItemRepository ItemRepository = new ItemRepository(context);

            Item i = new Item();
            var II = new ItemInstance() { Item = i};
            var b = new Borrow() { ItemInstance = II, User = new User(), ReturnDate = DateTime.Now.AddDays(-1) };
            var b1 = new Borrow() { ItemInstance = II, User = new User(), ReturnDate = DateTime.Now.AddDays(-2) };

            context.ItemInstances.Add(II);
            context.Borrows.Add(b);
            context.Borrows.Add(b1);


            context.SaveChanges();

            Assert.True(ItemRepository.isItemAvalable(i.Id, DateTime.Now));
            Assert.False(ItemRepository.isItemAvalable(i.Id, DateTime.Now.AddDays(-3)));
        }

        [Fact]
        public void TestItemInstacesFree()
        {
            AutomatContext context = SeededContext;
            ItemRepository ItemRepository = new ItemRepository(context);

            Item i = new Item();
            var II = new ItemInstance() { Item = i };
            var II2 = new ItemInstance() { Item = i };
            var b = new Borrow() { ItemInstance = II, User = new User(), ReturnDate = DateTime.Now.AddDays(-1) };
            var b1 = new Borrow() { ItemInstance = II, User = new User(), ReturnDate = DateTime.Now.AddDays(-2) };

            context.ItemInstances.Add(II);
            context.ItemInstances.Add(II2);

            context.Borrows.Add(b);
            context.Borrows.Add(b1);
            
            context.SaveChanges();


            var a = ItemRepository.getFreeItemInstances(i.Id, DateTime.Now);

            Assert.Equal(2,ItemRepository.getFreeItemInstances(i.Id, DateTime.Now).Count());
            Assert.Single(ItemRepository.getFreeItemInstances(i.Id, DateTime.Now.AddDays(-3)));
        }

        [Fact]
        public void TestItemInstaceFree()
        {
            AutomatContext context = SeededContext;
            ItemRepository ItemRepository = new ItemRepository(context);

            Item i = new Item();
            var II = new ItemInstance() { Item = i };
            var II2 = new ItemInstance() { Item = i };
            var b = new Borrow() { ItemInstance = II, User = new User(), ReturnDate = DateTime.Now.AddDays(-1) };
            var b1 = new Borrow() { ItemInstance = II, User = new User(), ReturnDate = DateTime.Now.AddDays(-2) };

            context.ItemInstances.Add(II);
            context.ItemInstances.Add(II2);

            context.Borrows.Add(b);
            context.Borrows.Add(b1);

            context.SaveChanges();


            var r1 = ItemRepository.getFreeItemInstance(i.Id, DateTime.Now);
            Assert.True(r1 == II || r1 == II2);
            r1 = ItemRepository.getFreeItemInstance(i.Id, DateTime.Now.AddDays(-3));
            Assert.True(r1 == II2);
        }


        [Fact]
        public void TestItemget()
        {
            AutomatContext context = SeededContext;
            ItemRepository ItemRepository = new ItemRepository(context);

            Item i = new Item();

            context.Items.Add(i);
            context.SaveChanges();

            Assert.Equal(i, ItemRepository.getItem(i.Id));
        }
    }
}



// Seed Service is in Application but I need it in the Repository test
//Code wiederholung verhindern(DbContext) in Tests
//User.Claims.First(c=>c.Issuer == "Name").Value ersetzen