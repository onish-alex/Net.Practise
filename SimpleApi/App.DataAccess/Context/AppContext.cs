using App.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace App.DataAccess.Context
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions options)
            : base(options)
        {
        }

        internal DbSet<Customer> Customers { get; set; }

        internal DbSet<Contract> Contracts { get; set; }

        internal DbSet<Delivery> Deliveries { get; set; }

        internal DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().Property(x => x.Cost).HasColumnType("money");
        }
    }
}