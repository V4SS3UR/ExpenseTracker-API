using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models.DTOs
{
    [NotMapped]
    public class ExpenseGetDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }

        public int ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }


        public ExpenseGetDTO(User user, ExpenseType expenseType)
        {
            this.ExpenseTypeId = expenseType.Id;
            this.ExpenseTypeName = expenseType.Name;

            this.UserId = user.Id;
            this.UserName = $"{user.FirstName} {user.LastName}";
        }

        public ExpenseGetDTO(Expense expense) : this(expense.User, expense.ExpenseType)
        {
            this.Id = expense.Id;
            this.Date = expense.Date;
            this.Amount = expense.Amount;
            this.Currency = expense.Currency;
            this.Comment = expense.Comment;
        }
    }
}
