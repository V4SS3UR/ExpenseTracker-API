using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Data
{
    public class ExpenseTypeEntityTypeConfiguration : IEntityTypeConfiguration<ExpenseType>
    {
        public void Configure(EntityTypeBuilder<ExpenseType> builder)
        {
            builder.Property(b => b.Name)
                .IsRequired();
        }
    }
}
