using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
            // Force database creation with InMemoryDatabase
            if (this.Database.IsInMemory())
            {
                this.Database.EnsureCreated();
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed initial users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Anthony", LastName = "Stark", Currency = "USD" },
                new User { Id = 2, FirstName = "Natasha", LastName = "Romanova", Currency = "RUB" }
            );

            // "Restaurant", "Hotel", or "Misc"
            modelBuilder.Entity<ExpenseType>().HasData(
                new ExpenseType { Id = 1, Name = "Restaurant", Description = string.Empty },
                new ExpenseType { Id = 2, Name = "Hotel", Description = string.Empty },
                new ExpenseType { Id = 3, Name = "Misc", Description = string.Empty }
            );

            //https://learn.microsoft.com/en-us/ef/core/modeling/
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new ExpenseEntityTypeConfiguration().Configure(modelBuilder.Entity<Expense>());
            new ExpenseTypeEntityTypeConfiguration().Configure(modelBuilder.Entity<ExpenseType>());
        }
    }
}
