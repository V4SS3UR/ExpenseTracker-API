using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Models.DTOs;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpensesController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // POST: api/expenses
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseCreateDTO expenseDTO)
        {
            try
            {
                var expense = new Expense
                {
                    Date = expenseDTO.Date,
                    Amount = expenseDTO.Amount,
                    Currency = expenseDTO.Currency,
                    Comment = expenseDTO.Comment,
                    UserId = expenseDTO.UserId,
                    ExpenseTypeId = expenseDTO.ExpenseTypeId
                };

                var createdExpense = await _expenseService.CreateExpenseAsync(expense);
                
                //return CreatedAtAction(nameof(GetExpenses), new { userId = expense.UserId }, createdExpense);
                return Ok(createdExpense);            
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/expenses?userId=1&sort=amount
        [HttpGet]
        public async Task<IActionResult> GetExpenses([FromQuery] int userId, [FromQuery] string sort = "date")
        {
            var expenses = await _expenseService.GetExpensesByUserAsync(userId, sort);
            return Ok(expenses);
        }
    }
}
