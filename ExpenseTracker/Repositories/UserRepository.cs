using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExpenseDbContext _context;

        public UserRepository(ExpenseDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetAllUsersAsync(string sortField = "lastName")
        {
            IQueryable<User> query = _context.Users;

            switch (sortField.ToLower())
            {
                case "currency":
                    query = query.OrderBy(e => e.Currency);
                    break;
                case "lastname":
                    query = query.OrderBy(e => e.LastName);
                    break;

                default:
                    throw new Exception("Unexpected sorting field.");
            }

            return await query.ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
