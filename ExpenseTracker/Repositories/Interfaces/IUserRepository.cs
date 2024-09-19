using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories
{
    public interface IUserRepository : IBaseRepository
    {
        Task<User> GetUserAsync(int userId);
        Task<List<User>> GetAllUsersAsync(string sortField = "lastName");
        Task<User> CreateUserAsync(User user);
    }
}
