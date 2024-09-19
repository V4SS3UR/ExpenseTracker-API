using ExpenseTracker.Models;
using ExpenseTracker.Models.DTOs;
using ExpenseTracker.Repositories;

namespace ExpenseTracker.Services
{
    public class ExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        private readonly IUserRepository _userRepository;

        public ExpenseService(IServiceProvider serviceProvider)
        {
            _expenseRepository = serviceProvider.GetService<IExpenseRepository>();
            _expenseTypeRepository = serviceProvider.GetService<IExpenseTypeRepository>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
        }

        public async Task<Expense> CreateExpenseAsync(Expense expense)
        {          
            var errors = new List<Exception>();

            // Validate: date cannot be in the future
            if (expense.Date > DateTime.Now)
                errors.Add(new Exception("Expense date cannot be in the future."));

            // Validate: date cannot be more than 3 months in the past
            if (expense.Date < DateTime.Now.AddMonths(-3))
                errors.Add(new Exception("Expense cannot be older than 3 months."));

            // Validate: comment is mandatory
            if (string.IsNullOrEmpty(expense.Comment))
                errors.Add(new Exception("Comment is required for the expense."));

            // Validate: currency is mandatory
            if (string.IsNullOrEmpty(expense.Currency))
                errors.Add(new Exception("Currency is required for the expense."));

            // Validate: currency is a valid ISO 4217 code
            var codes = ISO._4217.CurrencyCodesResolver.Codes.Select(o => o.Code);
            if (!codes.Contains(expense.Currency))
                errors.Add(new Exception("Currency is not a valid ISO 4217 code."));

            // Validate: no duplicate expenses for the same user (same date, same amount)
            if (await _expenseRepository.ExpenseExistsAsync(expense.UserId, expense.Date, expense.Amount))
                errors.Add(new Exception("Duplicate expense for the same date and amount."));

            // Validate: expense type must exist
            var expenseType = await _expenseTypeRepository.GetExpenseTypeAsync(expense.ExpenseTypeId);
            if (expenseType == null)
                errors.Add(new Exception("Expense type does not exist."));

            // Validate: user must exist
            var user = await _userRepository.GetUserAsync(expense.UserId);
            if (user == null)
            {
                errors.Add(new Exception("User does not exist."));
                throw new AggregateException("Validation error", errors);
            }

            // Validate: currency must match user's currency
            if (user.Currency != expense.Currency)
                errors.Add(new Exception("Expense currency must match the user's currency."));

            if (errors.Any())
            {
                throw new AggregateException("Validation error", errors);
            }

            // Affectations of fetch values to the expense object
            expense.User = user;
            expense.ExpenseType = expenseType;

            return await _expenseRepository.CreateExpenseAsync(expense);
        }

        public async Task<List<ExpenseGetDTO>> GetExpensesByUserAsync(int userId, string sortField = "date")
        {
            var expenses = await _expenseRepository.GetExpensesByUserAsync(userId, sortField);

            var expensesDTO = expenses.Select(e => new ExpenseGetDTO(e)).ToList();

            return expensesDTO;
        }
    }
}
