namespace PhoneBook.Data
{
    using Microsoft.EntityFrameworkCore;
    using PhoneBook.Models;

    public class PhoneBookDbContext : DbContext
    {
        public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<BookEntry> Entries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<BookEntry>().ToTable("BookEntries");
        }
    }
}
