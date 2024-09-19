using ExpenseTracker.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    [ Table("expenses"), 
      Index(nameof(Date), nameof(Amount), Name = "Date_Amount") ]
    public class Expense : IBaseEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }

        public int ExpenseTypeId { get; set; }
        public ExpenseType ExpenseType { get; set; } = null;

        public int UserId { get; set; } 
        public User User { get; set; } = null;

        public ExpenseGetDTO ToExpenseGetDTO() => new ExpenseGetDTO(this);
    }
}
