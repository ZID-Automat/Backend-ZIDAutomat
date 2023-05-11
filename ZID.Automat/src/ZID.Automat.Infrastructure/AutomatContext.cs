using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ZID.Automat.Domain.Models;
using System.Reflection.Metadata;
using ZID.Automat.Domain.Models.Logging;

namespace ZID.Automat.Infrastructure
{

    public class AutomatContext : DbContext
    {
        public AutomatContext(DbContextOptions<AutomatContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemInstance> ItemInstances => Set<ItemInstance>();
        public DbSet<Borrow> Borrows => Set<Borrow>();
        public DbSet<Categorie> Categories => Set<Categorie>();

        public DbSet<EjectedItemLog> EjectedItemLog => Set<EjectedItemLog>();
        public DbSet<InvalidQRCodeLog> InvalidQRCodeLogi => Set<InvalidQRCodeLog>();
        public DbSet<ScannedQRCodeLog> ScannedQRCodeLogi => Set<ScannedQRCodeLog>();




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
