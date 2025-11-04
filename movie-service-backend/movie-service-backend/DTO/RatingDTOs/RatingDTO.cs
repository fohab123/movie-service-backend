namespace movie_service_backend.DTO.RatingDTOs
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? FilmId { get; set; }
        public int? SeriesId { get; set; }
        public int Value { get; set; }
    }
}
