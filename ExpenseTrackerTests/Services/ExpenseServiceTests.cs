using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseTracker.Services.Tests
{
    [TestClass()]
    public class ExpenseServiceTests
    {
        private ExpenseService _expenseService => BuiltInMemoryDatabase();

        //Ensure a distinct database for each test
        private ExpenseService BuiltInMemoryDatabase()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ExpenseDbContext>(options => options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ExpenseService>();
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<ExpenseService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Expense date cannot be in the future.")]
        public async Task CreateExpense_ShouldThrowException_IfDateIsInFuture()
        {
            // Arrange
            var futureExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now.AddDays(1), // Future date
                Amount = 100,
                Comment = "Test expense",
                Currency = "USD",
                ExpenseTypeId = 1
            };

            // Act
            await _expenseService.CreateExpenseAsync(futureExpense);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Expense cannot be older than 3 months.")]
        public async Task CreateExpense_ShouldThrowException_IfDateIsOlderThanThreeMonths()
        {
            // Arrange
            var oldExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now.AddMonths(-4), // More than 3 months old
                Amount = 100,
                Comment = "Test expense",
                Currency = "USD",
                ExpenseTypeId = 1
            };

            // Act
            await _expenseService.CreateExpenseAsync(oldExpense);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Comment is required for the expense.")]
        public async Task CreateExpense_ShouldThrowException_IfCommentIsMissing()
        {
            // Arrange
            var invalidExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now,
                Amount = 100,
                Comment = "", // Empty comment
                Currency = "USD",
                ExpenseTypeId = 1
            };

            // Act
            await _expenseService.CreateExpenseAsync(invalidExpense);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Duplicate expense for the same date and amount.")]
        public async Task CreateExpense_ShouldThrowException_IfExpenseIsDuplicate()
        {
            var expenseService = _expenseService;

            // Arrange
            var duplicateExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now,
                Amount = 100,
                Comment = "Duplicate expense",
                Currency = "USD",
                ExpenseTypeId = 1
            };

            // Act
            await expenseService.CreateExpenseAsync(duplicateExpense);
            await expenseService.CreateExpenseAsync(duplicateExpense);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Expense type does not exist.")]
        public async Task CreateExpense_ShouldThrowException_IfExpenseTypeDoesNotExist()
        {
            // Arrange
            var invalidExpenseTypeExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now,
                Amount = 100,
                Comment = "Test expense",
                Currency = "USD",
                ExpenseTypeId = -1 // Invalid expense type
            };

            // Act
            await _expenseService.CreateExpenseAsync(invalidExpenseTypeExpense);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "User does not exist.")]
        public async Task CreateExpense_ShouldThrowException_IfUserDoesNotExist()
        {
            // Arrange
            var invalidUserExpense = new Expense
            {
                UserId = -1, // Invalid user
                Date = DateTime.Now,
                Amount = 100,
                Comment = "Test expense",
                Currency = "USD",
                ExpenseTypeId = 1
            };

            // Act
            await _expenseService.CreateExpenseAsync(invalidUserExpense);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Expense currency must match the user's currency.")]
        public async Task CreateExpense_ShouldThrowException_IfCurrencyDoesNotMatchUserCurrency()
        {
            // Arrange
            var invalidCurrencyExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now,
                Amount = 100,
                Comment = "Test expense",
                Currency = "EUR", // Wrong currency
                ExpenseTypeId = 1
            };

            // Act
            await _expenseService.CreateExpenseAsync(invalidCurrencyExpense);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Currency is required for the expense.")]
        public async Task CreateExpense_ShouldThrowException_IfCurrencyIsMissing()
        {
            // Arrange
            var invalidCurrencyExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now,
                Amount = 100,
                Comment = "Test expense",
                ExpenseTypeId = 1
            };

            // Act
            await _expenseService.CreateExpenseAsync(invalidCurrencyExpense);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Currency is invalid")]
        public async Task CreateExpense_ShouldThrowException_IfCurrencyIsInvalid()
        {
            // Arrange
            var invalidCurrencyExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now,
                Amount = 100,
                Comment = "Test expense",
                Currency = "Invalid", // Invalid currency
                ExpenseTypeId = 1
            };

            // Act
            await _expenseService.CreateExpenseAsync(invalidCurrencyExpense);
        }


        [TestMethod]
        public async Task CreateExpense_ShouldSucceed_IfValid()
        {
            // Arrange
            var validExpense = new Expense
            {
                UserId = 1,
                Date = DateTime.Now,
                Amount = 100,
                Comment = "Valid expense",
                Currency = "USD",
                ExpenseTypeId = 1
            };            

            // Act
            var createdExpense = await _expenseService.CreateExpenseAsync(validExpense);

            // Assert
            Assert.IsNotNull(createdExpense);
            Assert.AreEqual(validExpense.UserId, createdExpense.UserId);
            Assert.AreEqual(validExpense.ExpenseTypeId, createdExpense.ExpenseTypeId);
            Assert.AreEqual(validExpense.Date, createdExpense.Date);
            Assert.AreEqual(validExpense.Amount, createdExpense.Amount);
            Assert.AreEqual(validExpense.Comment, createdExpense.Comment);
            Assert.AreEqual(validExpense.Currency, createdExpense.Currency);
        }

        [TestMethod]
        public async Task GetExpensesByUserAsync_ShouldReturnExpenses_WhenValidUserId()
        {
            // Arrange
            int userId = 1;
            string sortField = "amount";         

            // Act
            var result = await _expenseService.GetExpensesByUserAsync(userId, sortField);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetExpensesByUserAsync_ShouldReturnEmptyList_WhenNoExpenses()
        {
            // Arrange
            int userId = 1;
            string sortField = "date";

            // Act
            var result = await _expenseService.GetExpensesByUserAsync(userId, sortField);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unexpected sorting field.")]
        public async Task GetExpensesByUserAsync_ShouldThrowException_WhenInvalidSortField()
        {
            // Arrange
            int userId = 1;
            string invalidSortField = "invalid";

            // Act
            await _expenseService.GetExpensesByUserAsync(userId, invalidSortField);
        }
    }
}