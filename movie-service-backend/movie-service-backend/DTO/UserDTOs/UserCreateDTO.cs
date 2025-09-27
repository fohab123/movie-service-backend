using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace movie_service_backend.DTO.UserDTOs
{
    public class UserCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        
        [Required]
        [MaxLength (50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        [DefaultValue("string")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>]).{6,}$",
            ErrorMessage = "Lozinka mora imati najmanje 6 karaktera, jedno veliko slovo, jedan broj i jedan specijalni karakter.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
        [Compare("Password", ErrorMessage = "Lozinke se ne poklapaju.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
