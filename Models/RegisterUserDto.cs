using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class RegisterUserDto
    {
        
        
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // id danej roli dla uzytkownika , domysle uzytkonik bedzie w roli user 
        public int RoleId { get; set; } = 1;
    }
}
