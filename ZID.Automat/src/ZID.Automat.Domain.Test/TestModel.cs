using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;
using Xunit;

namespace ZID.Automat.Domain.Test
{
    public class TestModel
    {
        private AutomatContext Context
        {
            get
            {
                DbContextOptions<AutomatContext> options = new DbContextOptionsBuilder<AutomatContext>()
                .UseSqlite("Data Source=TestDb1.db")
                .Options;

                AutomatContext db = new AutomatContext(options);
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                return db;
            }
        }

        [Fact]
        public void TestCreateDB()
        {
            AutomatContext db = Context;
            Assert.True(db.Database.CanConnect());
        }

        [Fact]
        public void TestAddUser()
        {
            AutomatContext db = Context;
            User user = new User() { Username = "TestUser", Joined = DateTime.Now };
            db.Users.Add(user);
            db.SaveChanges();
            Assert.True(db.Users.Count() == 1);
        }
        

        [Fact]
        public void TestAddCategorie()
        {
            AutomatContext db = Context;

            var cat = new Categorie() { Name = "Kabel", Description = "Kabel sind krass" };
            db.Categories.Add(cat);
            db.SaveChanges();
            
            Assert.Equal(1, db.Categories.Count());

            Assert.ThrowsAny<ArgumentNullException>(()=> cat.AddItemToCategorie(null));

            cat.AddItemToCategorie(new Item { Name = "Kabel", Description = "Das ist ein ganz besonderes Kabel", Image = "Hallo", Price = 99, SubName = "Subname" });

        }

        [Fact]
        public void TestAddItem()
        {
            AutomatContext db = Context;

            var cat = new Categorie() { Name = "Kabel", Description = "Kabel sind krass" };
            db.Categories.Add(cat);
            db.SaveChanges();

            Item item = new Item() { Name = "TestItem", Description = "Test Description", Image = "asd", Price = 100, Categorie = cat };
            db.Items.Add(item);
            db.SaveChanges();
            Assert.True(db.Items.Count() == 1);
        }

        [Fact]
        public void TestAddItemInstance()
        {
            AutomatContext db = Context;
            Item item = new Item()
            {
                Name = "Kabel1",
                Description = "This is a very cool thing",
                Image = "asd",
                Price = 100
            };
            db.Items.Add(item);

            ItemInstance II = new ItemInstance()
            {
                FirstAdded = DateTime.Now
            };
            item.AddItemInstance(II);
            db.SaveChanges();
            Assert.Equal(1, db.ItemInstances.Count());

            Assert.ThrowsAny<ArgumentNullException>(() => item.AddItemInstance(null));
            Assert.ThrowsAny<ArgumentException>(() => item.AddItemInstance(new ItemInstance() {FirstAdded = DateTime.Now.AddYears(1) }));
            Assert.ThrowsAny<ArgumentException>(() => item.AddItemInstance(new ItemInstance() {FirstAdded = DateTime.Now.AddYears(-1) }));
            
            item.AddItemInstance(new ItemInstance() { FirstAdded = DateTime.Now.AddMinutes(59) }) ;
            item.AddItemInstance(new ItemInstance() { FirstAdded = DateTime.Now.AddMinutes(-59) });

            db.SaveChanges();
            Assert.Equal(3, db.ItemInstances.Count());
        }

        [Fact]
        public void TestAdmonitionType()
        {
            AutomatContext db = Context;
            AdmonitionType Ad = new AdmonitionType() { Description = "Desc",Name="Name"};
            db.AdmonitionTypes.Add(Ad);
            db.SaveChanges();
            Assert.Equal(1, db.AdmonitionTypes.Count());
        }

        [Fact]
        public void TestBorrow()
        {
            AutomatContext db = Context;

            User user = new User() { Username = "TestUser", Joined = DateTime.Now };
            db.Users.Add(user);
            db.SaveChanges();

            Item item = new Item()
            {
                Name = "Kabel1",
                Description = "This is a very cool thing",
                Image = "asd",
                Price = 100
            };
            db.Items.Add(item);

            ItemInstance II = new ItemInstance()
            {
                FirstAdded = DateTime.Now
            };
            item.AddItemInstance(II);
            db.SaveChanges();

            Borrow b = new Borrow
            {
                BorrowDate = DateTime.Now,
                ItemInstance = II,
                PredictedReturnDate = DateTime.Now.AddDays(3),
                User = user,
                Item = item
            };
            db.Borrows.Add(b);
            db.SaveChanges();
            b.ReturnDate = DateTime.Now.AddDays(1);
            db.SaveChanges();
            Assert.Equal(1, db.Borrows.Count());
        }

        [Fact]
        public void TestUserAddBorrow()
        {
            AutomatContext db = Context;

            User user = new User() { Username = "TestUser", Joined = DateTime.Now };
            db.Users.Add(user);
            db.SaveChanges();

            Item item = new Item()
            {
                Name = "Kabel1",
                Description = "This is a very cool thing",
                Image = "asd",
                Price = 100
            };
            db.Items.Add(item);

            ItemInstance II = new ItemInstance()
            {
                FirstAdded = DateTime.Now
            };
            item.AddItemInstance(II);
            db.SaveChanges();

            Borrow b = new Borrow{ Item = item, BorrowDate = DateTime.Now,ItemInstance = II, PredictedReturnDate = DateTime.Now.AddDays(3)};
            user.AddBorrow(b);
            db.SaveChanges();
            b.ReturnDate = DateTime.Now.AddDays(1);
            db.SaveChanges();

            Assert.ThrowsAny<ArgumentNullException>(() =>user.AddBorrow(null));
            Assert.ThrowsAny<ArgumentException>(() =>user.AddBorrow(new Borrow { BorrowDate = DateTime.Now.AddDays(2), ItemInstance = II, PredictedReturnDate = DateTime.Now.AddDays(3) }));
            Assert.ThrowsAny<ArgumentException>(() =>user.AddBorrow(new Borrow { BorrowDate = DateTime.Now.AddDays(-2), ItemInstance = II, PredictedReturnDate = DateTime.Now.AddDays(3) }));

            user.AddBorrow(new Borrow { BorrowDate = DateTime.Now.AddMinutes(59), ItemInstance = II, PredictedReturnDate = DateTime.Now.AddDays(3) });
            user.AddBorrow(new Borrow { BorrowDate = DateTime.Now.AddMinutes(-59), ItemInstance = II, PredictedReturnDate = DateTime.Now.AddDays(3) });

            Assert.ThrowsAny<ArgumentException>(() => user.AddBorrow(new Borrow { BorrowDate = DateTime.Now, ItemInstance = II, PredictedReturnDate = DateTime.Now.AddDays(3), ReturnDate= DateTime.Now }));
            Assert.ThrowsAny<ArgumentException>(() => user.AddBorrow(new Borrow { BorrowDate = DateTime.Now, ItemInstance = II, PredictedReturnDate = DateTime.Now.AddDays(-3), }));

            Assert.Equal(1, db.Borrows.Count());
        }

        [Fact]
        public void TestAdmonition()
        {
            AutomatContext db = Context;

            User user = new User() { Username = "TestUser", Joined = DateTime.Now };
            db.Users.Add(user);
            db.SaveChanges();

            Item item = new Item()
            {
                Name = "Kabel1",
                Description = "This is a very cool thing",
                Image = "asd",
                Price = 100
            };
            db.Items.Add(item);

            ItemInstance II = new ItemInstance()
            {
                FirstAdded = DateTime.Now
            };
            item.AddItemInstance(II);
            db.SaveChanges();

            Borrow b = new Borrow {Item=item, BorrowDate = DateTime.Now, ItemInstance = II, PredictedReturnDate = DateTime.Now.AddDays(3) };
            user.AddBorrow(b);
            db.SaveChanges();
            b.ReturnDate = DateTime.Now.AddDays(1);
            db.SaveChanges();

            AdmonitionType Ad = new AdmonitionType() { Description = "Desc", Name = "Name" };
            db.AdmonitionTypes.Add(Ad);
            db.SaveChanges();
            Assert.Equal(1, db.AdmonitionTypes.Count());

            b.AddAdmonition(new Admonition() { AdmonitionDate = DateTime.Now.AddDays(1), AdmonitionType = Ad, Comment = "Very good" });
            db.SaveChanges();
            Assert.Equal(1, db.Admonitions.Count());

            Assert.ThrowsAny<ArgumentNullException>(()=>b.AddAdmonition(null));
            Assert.ThrowsAny<ArgumentException>(()=> b.AddAdmonition(new Admonition() { AdmonitionDate = DateTime.Now.AddDays(-100), AdmonitionType = Ad, Comment = "Very good" }));
        }
    }
}