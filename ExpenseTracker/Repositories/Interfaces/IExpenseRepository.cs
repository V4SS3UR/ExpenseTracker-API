using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories
{
    public interface IExpenseRepository : IBaseRepository
    {
        Task<List<Expense>> GetExpensesByUserAsync(int userId, string sortField = "date");
        Task<Expense> CreateExpenseAsync(Expense expense);
        Task<bool> ExpenseExistsAsync(int userId, DateTime date, decimal amount);
    }
}
