namespace movie_service_backend.DTO.FilmDTOs
{
    public class RecommendedFilmDTO
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public double Score { get; set; }
        public double GlobalRating { get; set; }
    }
}
