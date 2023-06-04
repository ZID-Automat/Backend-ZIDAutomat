using Bogus;
using System.Text.Json;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Application
{

    public class SeedService : ISeedService
    {
        private readonly AutomatContext _context;
        public SeedService(AutomatContext context)
        {
            _context = context;
        }


        public void Seed()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var text = File.ReadAllText(@"./SeedingData/ItemSeeding.json");
            var items = JsonSerializer.Deserialize<IEnumerable<ItemJsonData>>(text);
            var categories = JsonSerializer.Deserialize<IEnumerable<CatJsonData>>(File.ReadAllText(@"./SeedingData/CategoriesSeeding.json"));
            
            var DBCats = categories.Select((c, index) => new Categorie() { Id=index +1,Name = c.Name, Description = c.Description });
            _context.Categories.AddRange(DBCats);
            var DbITems = items.Select((i,index) => {
                string image = i.Image.Substring(6);
               return new Item() { Id = index + 1, CategorieId = i.CategorieId, Description = i.Description, Image = "https://github.com/ZID-Automat/Backend-ZIDAutomat/tree/main/ZID.Automat/src/ZID.Automat.Api/"+image, LocationImAutomat = "", SubName = i.SubName, Price = i.Price, Name = i.Name };

            }
            );



            DbITems.ToList().ForEach(item =>
            {
                for (int j = 0; j < 20; j++)
                {
                    item.ItemInstances.Add(new ItemInstance()
                    {
                        Item = item,
                        FirstAdded = DateTime.Now.AddDays(new Random().NextDouble() * -10 - 10)
                    });
                }
            });
            _context.Items.AddRange(DbITems);

            _context.SaveChanges();

            var Users = new Faker<User>()
                .RuleFor(u=>u.Id,f=>f.IndexFaker+1)
                .RuleFor(u => u.Vorname, f => f.Person.FirstName)
                .RuleFor(u => u.Nachname, f => f.Person.LastName)
                .RuleFor(u => u.Name, (f, u) => u.Nachname.Substring(0, 3) + (20000 + f.IndexFaker+1))
                .RuleFor(u => u.Blockiert, f => f.Random.Bool(0.07f))
                .RuleFor(u => u.Joined, f => f.Date.Between(DateTime.Now, DateTime.Now.AddDays(-70)))
                .RuleFor(u => u.LastLogin, f => GenerateRandomDate(f))
                .RuleFor(u=>u.Borrow, f =>
                {
                    List<Borrow> borrows = new List<Borrow>();
                    for (int i = 0; i < f.Random.Int(10, 70); i++)
                    {
                        var borrowDate = DateTime.Now.AddDays(-f.Random.Int(0, 360));
                        var preReturn = borrowDate.AddDays(f.Random.Int(2, 5));
                        var CollDate = borrowDate.AddHours(f.Random.Int(1, 4));

                        var returnidatei = DateTime.Now;
                        var entsch = false;

                        var state = f.Random.Float();
                        if(state < 0.15f)
                        {
                            returnidatei = preReturn.AddDays(f.Random.Int(2, 10));
                        }
                        else if (state < 0.20)
                        {
                            returnidatei = preReturn.AddDays(f.Random.Int(2, 10));
                            entsch = true;
                        }
                        else 
                        {
                            returnidatei = preReturn.AddDays(f.Random.Int(-1, -2));

                        }

                        var borrow = new Borrow()
                        {
                            GUID = f.Random.Guid(),
                            ItemInstance = new ItemInstance()
                            {
                                ItemId = f.PickRandom(DbITems).Id,
                                FirstAdded = borrowDate.AddDays(-f.Random.Int(4,10))
                            },
                            entschuldigt = entsch,
                            BorrowDate = borrowDate,
                            ReturnDate = returnidatei,
                            PredictedReturnDate = preReturn,
                            CollectDate = CollDate,
                        };
                        borrows.Add(borrow);
                    }
                    return borrows;
                })
                .Generate(40);

            _context.Users.AddRange(Users);


            _context.SaveChanges();


        }

        DateTime GenerateRandomDate(Faker faker)
        {
            var randomValue = faker.Random.Double();

            var now = DateTime.Now;
                 
            if (randomValue <= 0.10)
            {
                // 10% der Fälle: Bereich 1
                return faker.Date.Between(now.AddDays(0),now.AddDays(-1));
            }
            else if (randomValue <= 0.30)
            {
                // 20% der Fälle: Bereich 2
                return faker.Date.Between(now.AddDays(-1.1), now.AddDays(-3));
            }
            else if (randomValue <= 0.60)
            {
                // 30% der Fälle: Bereich 3
                return faker.Date.Between(now.AddDays(-3.1),now.AddDays(-7));
            }
            else if (randomValue <= 0.75)
            {
                // 15% der Fälle: Bereich 4
                return faker.Date.Between(now.AddDays(-7.1),now.AddDays(-14));
            }
            else if (randomValue <= 0.85)
            {
                // 10% der Fälle: Bereich 5
                return faker.Date.Between(now.AddDays(-14.1),now.AddDays(-30));
            }
            else
            {
                // 5% der Fälle: Bereich 6
                return faker.Date.Between(now.AddDays(-30.1),now.AddDays(-80));
            }
        }
    }
    internal class ItemJsonData
    {
        public string Name { get; set; }
        public string SubName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string LocationImAutomat { get; set; }
        public int CategorieId { get; set; }
    }

    internal class CatJsonData
    {
        public string Description { get; set; }
        public string Name { get; set; }
    }



    public interface ISeedService
    {
        void Seed();
    }
}