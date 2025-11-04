namespace movie_service_backend.DTO.FilmDTOs
{
    public class FilmGenreGroupDTO
    {
        public GenreDTO Genre {  get; set; }
        public List<FilmDTO> Films { get; set; }
    }
}
