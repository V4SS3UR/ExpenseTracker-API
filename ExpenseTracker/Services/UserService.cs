using ExpenseTracker.Models;
using ExpenseTracker.Repositories;

namespace ExpenseTracker.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IServiceProvider serviceProvider)
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
        }

        public async Task<List<User>> GetAllUsersAsync(string sortField = "lastName")
        {
            return await _userRepository.GetAllUsersAsync(sortField);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var errors = new List<Exception>();

            // Validate: firstName is mandatory
            if (string.IsNullOrEmpty(user.FirstName))
                errors.Add(new Exception("First name is required."));

            // Validate: lastName is mandatory
            if (string.IsNullOrEmpty(user.LastName))
                errors.Add(new Exception("Last name is required."));

            // Validate: currency is mandatory
            if (string.IsNullOrEmpty(user.Currency))
                errors.Add(new Exception("Currency is required."));

            // Validate: currency is a valid ISO 4217 code
            var codes = ISO._4217.CurrencyCodesResolver.Codes.Select(o => o.Code);
            if (!codes.Contains(user.Currency))
                errors.Add(new Exception("Currency is not a valid ISO 4217 code."));

            if (errors.Any())
            {
                throw new AggregateException("Validation error", errors);
            }

            return await _userRepository.CreateUserAsync(user);
        }        
    }
}
