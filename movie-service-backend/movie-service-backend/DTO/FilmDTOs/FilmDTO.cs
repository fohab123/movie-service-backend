namespace movie_service_backend.DTO.FilmDTOs
{
    public class FilmDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public int Duration { get; set; }
        public string PosterUrl { get; set; }
    }

}
