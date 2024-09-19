using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Data
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(b => b.FirstName)
                .IsRequired();

            builder.Property(b => b.LastName)
                .IsRequired();

            builder.Property(b => b.Currency)
                .HasMaxLength(3)
                .IsRequired();                
        }
    }
}
