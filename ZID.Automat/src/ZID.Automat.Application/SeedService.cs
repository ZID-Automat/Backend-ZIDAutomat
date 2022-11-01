using Bogus;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Application
{
    public class SeedService : ISeedService
    {
        private readonly AutomatContext Context;

        public SeedService(AutomatContext automatContext)
        {
            Context = automatContext;
        }

        public void Seed()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            var cats = SeedCategories(20);
            var items = SeedItems(100, cats);
            var itemInstances = SeedItemInstances(500, items);
            var users = SeedUsers(20);

            var borrows = SeedBorrows(100, users, itemInstances);

            var admonitionTypes = SeedAdmonitionTypes(5);
            var admonition = SeedAdmonitions(100,admonitionTypes, borrows);

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
                .RuleFor(i => i.Description, f => f.Lorem.Sentence(25))
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