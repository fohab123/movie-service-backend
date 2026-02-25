namespace movie_service_backend.DTO.CommentDTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int? FilmId { get; set; }
        public int? SeriesId { get; set; }
    }
}
