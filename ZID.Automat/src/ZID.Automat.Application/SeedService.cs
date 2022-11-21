using Bogus;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Application
{
    
    public class SeedService : ISeedService
    {
        public const int CATE = 20;
        public const int ITEMS = 20;
        public const int ITEMSINSTANCE = 100;
        public const int USERS = 20;
        public const int BORROWS = 00;
        public const int ADMONTYPE = 20;
        public const int ADMON = 20;

        private readonly AutomatContext Context;

        public SeedService(AutomatContext automatContext)
        {
            Context = automatContext;
        }

        public void Seed()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            var cats = SeedCategories(CATE);
            var items = SeedItems(ITEMS, cats);
            var itemInstances = SeedItemInstances(ITEMSINSTANCE, items);
            var users = SeedUsers(USERS);

            var borrows = SeedBorrows(BORROWS, users, itemInstances);

            var admonitionTypes = SeedAdmonitionTypes(ADMONTYPE);
            var admonition = SeedAdmonitions(ADMON,admonitionTypes, borrows);

            Context.Categories.AddRange(cats);
            Context.Items.AddRange(items);
            Context.ItemInstances.AddRange(itemInstances);
            Context.Users.AddRange(users);
            Context.Borrows.AddRange(borrows);
            Context.AdmonitionTypes.AddRange(admonitionTypes);
            Context.Admonitions.AddRange(admonition);

            Context.SaveChanges();
            
        }

        private List<Categorie> SeedCategories(int count)
        {
            return new Faker<Categorie>()
                 .RuleFor(c => c.Name, f => f.Commerce.Categories(1).First())
                 .RuleFor(c => c.Description, f => f.Lorem.Sentence(10))
                 .Generate(count);
        }

        private List<Item> SeedItems(int count, List<Categorie> categories)
        {
            var ItemName = new[] { "LanKabel", "Powerbank", "RaspberryPi3", "KopfhörerInEar", "KopfährerOverEar", "Batterien", "UsbKabel", "USBCKabel", "Mikro USB Kabel", "Lautsprecher" };
            return new Faker<Item>()
                .RuleFor(i => i.Name, f => f.PickRandom(ItemName))
                .RuleFor(i => i.Description, f => f.Lorem.Sentence(10))
                .RuleFor(i => i.Image, f => f.Image.LoremFlickrUrl())
                .RuleFor(i => i.Price, f => f.Random.Decimal(10, 100))
                .RuleFor(i => i.SubName, f => f.Lorem.Sentence(5))
                .RuleFor(i => i.Categorie, f => f.PickRandom(categories))
                .Generate(count);
        }

        private List<ItemInstance> SeedItemInstances(int count, List<Item> Items)
        {
            return new Faker<ItemInstance>()
                .RuleFor(i => i.Item, f => f.PickRandom(Items))
                .RuleFor(i => i.FirstAdded, f => f.Date.Past())
                .Generate(count);
        }

        private List<User> SeedUsers(int count)
        {
            return new Faker<User>()
                    .RuleFor(u => u.Username, f => f.Internet.UserName())
                    .RuleFor(u => u.Joined, f => f.Date.Past())
                    .Generate(count);
        }

        private List<AdmonitionType> SeedAdmonitionTypes(int count)
        {
            return new Faker<AdmonitionType>()
                .RuleFor(a => a.Name, f => f.Commerce.Categories(1).First())
                .RuleFor(a => a.Description, f => f.Lorem.Sentence(10))
                .Generate(count);
        }

        private List<Borrow> SeedBorrows(int count, List<User> users, List<ItemInstance> itemInstances)
        {
            return new Faker<Borrow>()
                .RuleFor(b => b.User, f => f.PickRandom(users))
                .RuleFor(b => b.ItemInstance, f => f.PickRandom(itemInstances))
                .RuleFor(b => b.BorrowDate, f => f.Date.Past())
                .RuleFor(b => b.ReturnDate, f => f.Date.Future())
                .Generate(count);
        }

        private List<Admonition> SeedAdmonitions(int count, List<AdmonitionType> admonitionTypes, List<Borrow> borrows)
        {
            if (borrows.Count == 0) return new List<Admonition>();
            return new Faker<Admonition>()
                .RuleFor(a => a.AdmonitionType, f => f.PickRandom(admonitionTypes))
                .RuleFor(a => a.Comment, f => f.Lorem.Sentence(10))
                .RuleFor(a => a.Borrow, f => f.PickRandom(borrows))
                .RuleFor(a => a.AdmonitionDate, f => f.Date.Past())
                .Generate(count);
        }
    }

    public interface ISeedService
    {
        void Seed();
    }
}