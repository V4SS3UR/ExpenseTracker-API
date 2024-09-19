using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    [Table("users")]
    public class User : IBaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Currency { get; set; }

        public virtual IEnumerable<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
