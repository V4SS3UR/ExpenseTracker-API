using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories;
using ExpenseTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpenseTracker.Tests.Services
{
    [TestClass]
    public class ExpenseTypeServiceTests
    {
        private ExpenseTypeService _expenseTypeService => BuiltInMemoryDatabase();

        //Ensure a distinct database for each test
        private ExpenseTypeService BuiltInMemoryDatabase()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ExpenseDbContext>(options => options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ExpenseTypeService>();
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<ExpenseTypeService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }      

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Name is required.")]
        public async Task CreateExpenseTypeAsync_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            var expenseType = new ExpenseType { Name = "" }; // Invalid: empty name

            // Act
            await _expenseTypeService.CreateExpenseTypeAsync(expenseType);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException), "Expense type already exists.")]
        public async Task CreateExpenseTypeAsync_ShouldThrowException_WhenExpenseTypeExists()
        {
            // Arrange
            var expenseType = new ExpenseType { Name = "Restaurant" };

            // Act
            await _expenseTypeService.CreateExpenseTypeAsync(expenseType);
        }

        [TestMethod]
        public async Task GetAllExpensesTypeAsync_ShouldReturnAllExpenseTypes()
        {
            // Act
            var result = await _expenseTypeService.GetAllExpenseTypesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task CreateExpenseTypeAsync_ShouldCreateExpenseType_WhenValid()
        {
            // Arrange
            var expenseType = new ExpenseType { Name = "AnOtherTypeOfExpense", Description = "" };

            // Act
            var result = await _expenseTypeService.CreateExpenseTypeAsync(expenseType);

            // Assert
            Assert.AreEqual(expenseType, result);
        }
    }
}
