using movie_service_backend.DTO.FilmDTOs;

namespace movie_service_backend.DTO.SeriesDTOs
{
    public class SeriesGenreGroupDTO
    {
        public GenreDTO Genre { get; set; }
        public List<SeriesDTO> Series { get; set; }
    }
}
