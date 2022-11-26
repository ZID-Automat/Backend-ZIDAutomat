using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Application;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Repository.Test
{
    public class TestBorrowRepository
    {
        private AutomatContext SeededContext
        {
            get
            {
                DbContextOptions<AutomatContext> options = new DbContextOptionsBuilder<AutomatContext>()
                .UseSqlite("Data Source=Test1Db.db")
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
        public void TestGetActiveQrCodes()
        {
            var BorrowRepo = new BorrowRepository(SeededContext);
            foreach(var borrows in BorrowRepo.getActiveBorrows())
            {
                Assert.Null(borrows.CollectDate);
            }
        }
    }
}
