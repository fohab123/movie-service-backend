using System.ComponentModel.DataAnnotations;

namespace movie_service_backend.DTO.RatingDTOs
{
    public class RatingCreateFilmDTO
    {
        [Required]
        public int UserId { get; set; }

        public int FilmId { get; set; }
       

        [Range(1, 10)]
        public int Value { get; set; }
    }
}
