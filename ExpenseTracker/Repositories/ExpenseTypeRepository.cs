using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class ExpenseTypeRepository : IExpenseTypeRepository
    {
        private readonly ExpenseDbContext _context;

        public ExpenseTypeRepository(ExpenseDbContext context)
        {
            _context = context;
        }

        public async Task<ExpenseType> GetExpenseTypeAsync(int id)
        {
            return await _context.ExpenseTypes.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<ExpenseType>> GetAllExpenseTypesAsync()
        {
            IQueryable<ExpenseType> query = _context.ExpenseTypes;
            return await query.ToListAsync();
        }

        public async Task<ExpenseType> CreateExpenseTypeAsync(ExpenseType expenseType)
        {
            _context.ExpenseTypes.Add(expenseType);
            await _context.SaveChangesAsync();
            return expenseType;

        }

        public async Task<bool> ExpenseTypeExistsAsync(string name)
        {
            return await _context.ExpenseTypes.AnyAsync(e => e.Name == name);
        }
    }
}
