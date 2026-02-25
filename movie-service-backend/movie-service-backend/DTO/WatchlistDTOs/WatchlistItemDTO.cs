namespace movie_service_backend.DTO.WatchlistDTOs
{
    public class WatchlistItemDTO
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Duration { get; set; }
        public double Rating { get; set; }
        public string VotesText { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public List<string> Stars { get; set; } = new();
        public string PosterUrl { get; set; }
        public bool Watched { get; set; }
        public string AddedAt { get; set; }
        public List<string> Genres { get; set; } = new();
    }
}
