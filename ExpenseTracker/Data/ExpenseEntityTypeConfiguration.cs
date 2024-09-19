using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Data
{
    public class ExpenseEntityTypeConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.Property(b => b.Comment)
                .IsRequired();

            builder.Property(b => b.Amount)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}
