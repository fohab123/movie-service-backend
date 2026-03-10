using System.ComponentModel.DataAnnotations;

namespace movie_service_backend.DTO.UserDTOs
{
    public class UpdateEmailDTO
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
