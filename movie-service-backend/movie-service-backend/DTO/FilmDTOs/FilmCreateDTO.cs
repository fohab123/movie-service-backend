using System.ComponentModel.DataAnnotations;

namespace movie_service_backend.DTO.FilmDTOs
{
    public class FilmCreateDTO
    {
        
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Range(1888, 2100)]
        public int Year { get; set; }

        [Required]
        public int GenreId { get; set; }

        [MaxLength(100)]
        public string Director { get; set; }

        [Range(1, 500)]
        public int Duration { get; set; }

        [MaxLength(500)]
        public string PosterUrl { get; set; } // opcionalno
    }

}
