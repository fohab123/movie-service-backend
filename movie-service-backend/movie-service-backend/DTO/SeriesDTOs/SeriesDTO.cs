using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.Models;

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
        public DateTime CreatedAt {  get; set; }

        public GenreDTO Genre { get; set; }
    }
}
