namespace PhoneBook.Data
{
    using System;
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

        public DbSet<BookEntryStatus> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<BookEntry>().ToTable("BookEntries");
            modelBuilder.Entity<BookEntryStatus>().ToTable("BookEntryStatuses");

            modelBuilder.Entity<BookEntryStatus>().HasData(
                new BookEntryStatus()
                {
                    Id = Guid.NewGuid(),
                    Name = "Актуально",
                },
                new BookEntryStatus()
                {
                    Id = Guid.NewGuid(),
                    Name = "Требует подтверждения",
                },
                new BookEntryStatus()
                {
                    Id = Guid.NewGuid(),
                    Name = "Нектуально",
                });
        }
    }
}
