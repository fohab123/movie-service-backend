using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movie_service_backend.Models
{
    public class User
    {
        [Key] // označava primarni ključ
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto-increment
        public int Id { get; set; }

        [Required] // obavezno polje
        [MaxLength(50)] // opciono ograničenje dužine
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public bool IsAdmin { get; set; } = false;
    }
}
 