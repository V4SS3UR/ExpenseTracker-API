using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models.DTOs
{
    [NotMapped]
    public class UserCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Currency { get; set; }
    }
}
