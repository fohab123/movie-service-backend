using movie_service_backend.Models;

namespace movie_service_backend.DTO.FilmDTOs
{
    public class FilmDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public int Duration { get; set; }
        public string Director { get; set; }
        public string PosterUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public double? Rating { get; set; }

        // Navigacioni DTO
        public ICollection<GenreDTO> Genres { get; set; }
    }

}
