using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseDbContext _context;

        public ExpenseRepository(ExpenseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetExpensesByUserAsync(int userId, string sortField = "date")
        {
            var query = _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.User)           
                .Include(e => e.ExpenseType)
                .AsQueryable();    

            // Apply sorting
            switch (sortField.ToLower())
            {
                case "amount":
                    query = query.OrderBy(e => e.Amount);
                    break;
                case "date":
                    query = query.OrderBy(e => e.Date);
                    break;

                default:
                    throw new Exception("Unexpected sorting field.");
            }

            return await query.ToListAsync();
        }

        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<bool> ExpenseExistsAsync(int userId, DateTime date, decimal amount)
        {
            return await _context.Expenses.AnyAsync(e => 
                e.UserId == userId && 
                e.Date == date && 
                e.Amount == amount);
        }
    }
}
