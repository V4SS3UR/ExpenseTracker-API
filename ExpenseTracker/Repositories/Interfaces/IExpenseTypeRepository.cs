using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories
{
    public interface IExpenseTypeRepository : IBaseRepository
    {
        Task<ExpenseType> GetExpenseTypeAsync(int id);
        Task<List<ExpenseType>> GetAllExpenseTypesAsync();
        Task<ExpenseType> CreateExpenseTypeAsync(ExpenseType expenseType);
        Task<bool> ExpenseTypeExistsAsync(string name);
    }
}
