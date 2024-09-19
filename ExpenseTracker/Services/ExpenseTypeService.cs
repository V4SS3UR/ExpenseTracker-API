using ExpenseTracker.Models;
using ExpenseTracker.Repositories;

namespace ExpenseTracker.Services
{
    public class ExpenseTypeService
    {
        private readonly IExpenseTypeRepository _expenseTypeRepository;

        public ExpenseTypeService(IServiceProvider serviceProvider)
        {
            _expenseTypeRepository = serviceProvider.GetService<IExpenseTypeRepository>();
        }

        public async Task<ExpenseType> CreateExpenseTypeAsync(ExpenseType expenseType)
        {
            var errors = new List<Exception>();

            // Validate: name is mandatory
            if (string.IsNullOrEmpty(expenseType.Name))
                errors.Add(new Exception("Name is required."));

            // Validate: name must be unique
            if (await _expenseTypeRepository.ExpenseTypeExistsAsync(expenseType.Name))
                errors.Add(new Exception("Expense type already exists."));

            if (errors.Any())
            {
                throw new AggregateException("Validation error", errors);
            }

            return await _expenseTypeRepository.CreateExpenseTypeAsync(expenseType);
        }

        public async Task<List<ExpenseType>> GetAllExpenseTypesAsync()
        {
            return await _expenseTypeRepository.GetAllExpenseTypesAsync();
        }
    }
}
