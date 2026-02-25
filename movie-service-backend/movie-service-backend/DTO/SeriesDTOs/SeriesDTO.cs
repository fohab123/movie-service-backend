using movie_service_backend.DTO.FilmDTOs;

namespace movie_service_backend.DTO.SeriesDTOs
{
    public class SeriesDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public int Seasons { get; set; }
        public string Director { get; set; }
        public string PosterUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public double? Rating { get; set; }

        public ICollection<GenreDTO> Genres { get; set; }
    }
}