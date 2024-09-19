using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Models.DTOs;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userDTO)
        {
            try
            {
                var user = new User
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Currency = userDTO.Currency
                };

                var createdExpense = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUsers), createdExpense);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/users?sort=lastName
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string sort = "lastName")
        {
            var users = await _userService.GetAllUsersAsync(sort);
            return Ok(users);
        }
    }
}
