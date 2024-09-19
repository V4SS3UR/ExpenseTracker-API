using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories;
using ExpenseTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseTracker.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService _userService => BuiltInMemoryDatabase();

        //Ensure a distinct database for each test
        private UserService BuiltInMemoryDatabase()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ExpenseDbContext>(options => options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<UserService>();
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<UserService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task CreateUserAsync_ShouldThrowException_WhenFirstNameIsMissing()
        {
            // Arrange
            var user = new User { LastName = "Stark", Currency = "USD" };

            // Act
            await _userService.CreateUserAsync(user);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task CreateUserAsync_ShouldThrowException_WhenLastNameIsMissing()
        {
            // Arrange
            var user = new User { FirstName = "Anthony", Currency = "USD" };

            // Act
            await _userService.CreateUserAsync(user);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task CreateUserAsync_ShouldThrowException_WhenCurrencyIsMissing()
        {
            // Arrange
            var user = new User { FirstName = "Anthony", LastName = "Stark" };

            // Act
            await _userService.CreateUserAsync(user);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task CreateUserAsync_ShouldThrowException_WhenInvalidCurrency()
        {
            // Arrange
            var user = new User { FirstName = "Anthony", LastName = "Stark", Currency = "Invalid" };

            // Act
            await _userService.CreateUserAsync(user);
        }

        [TestMethod]
        public async Task CreateUserAsync_ShouldReturnUser_WhenValidUserIsProvided()
        {
            // Arrange
            var user = new User { FirstName = "Anthony", LastName = "Stark", Currency = "USD" };

            // Act
            var result = await _userService.CreateUserAsync(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Anthony", result.FirstName);
            Assert.AreEqual("Stark", result.LastName);
            Assert.AreEqual("USD", result.Currency);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_ShouldReturnSortedUsers()
        {
            // Act
            var result = await _userService.GetAllUsersAsync("lastName");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Romanova", result[0].LastName);
            Assert.AreEqual("Stark", result[1].LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetAllUsersAsync_ShouldThrowException_WhenInvalidSortField()
        {
            // Act
            await _userService.GetAllUsersAsync("invalid");
        }
    }
}
