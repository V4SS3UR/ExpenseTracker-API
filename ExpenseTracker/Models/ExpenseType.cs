using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    [ Table("expenseTypes")]
    public class ExpenseType : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
