using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First name must start with a capital letter and contain only alphabets.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Last name must start with a capital letter and contain only alphabets.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [CustomDateOfBirthValidation(ErrorMessage = "You must be at least 18 years old.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
