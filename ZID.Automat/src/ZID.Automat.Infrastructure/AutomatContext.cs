using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;

namespace ZID.Automat.Infrastructure
{
    public class AutomatContext : DbContext
    {
        public AutomatContext(DbContextOptions<AutomatContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemInstance> ItemInstances => Set<ItemInstance>();
        public DbSet<Borrow> Borrows => Set<Borrow>();
        public DbSet<Admonition> Admonitions => Set<Admonition>();
        public DbSet<AdmonitionType> AdmonitionTypes => Set<AdmonitionType>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(e => e.Username);
        }
    }
}
