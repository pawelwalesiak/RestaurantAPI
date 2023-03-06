using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class RegisterUserDto
    {
        public string? FirstName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // id danej roli dla uzytkownika , domysle uzytkonik bedzie w roli user 
        public int RoleId { get; set; } = 1;
    }
}
