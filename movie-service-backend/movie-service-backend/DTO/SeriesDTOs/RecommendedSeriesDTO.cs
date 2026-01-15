namespace movie_service_backend.DTO.SeriesDTOs
{
    public class RecommendedSeriesDTO
    {
        public int SeriesId { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public double Score { get; set; }
        public double GlobalRating { get; set; }
    }
}
