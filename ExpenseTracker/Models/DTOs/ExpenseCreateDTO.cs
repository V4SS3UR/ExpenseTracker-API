using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models.DTOs
{
    [NotMapped]
    public class ExpenseCreateDTO
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }

        public int UserId { get; set; }
        public int ExpenseTypeId { get; set; }
    }
}
