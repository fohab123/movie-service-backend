using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movie_service_backend.Models
{
    public class User
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Required] 
        [MaxLength(50)] 
        public string FirstName { get; set; }

        [Required] 
        [MaxLength(50)] 
        public string LastName { get; set; }

        [Required] 
        [MaxLength(50)] 
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
 