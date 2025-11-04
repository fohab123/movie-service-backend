namespace movie_service_backend.DTO.SeriesDTOs
{
    public class SeriesCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Seasons { get; set; }
        public string Director { get; set; }
        public string PosterUrl { get; set; }
        public int GenreId { get; set; }
    }
}
