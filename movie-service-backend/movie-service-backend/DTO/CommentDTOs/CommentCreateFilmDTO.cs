namespace movie_service_backend.DTO.CommentDTOs
{
    public class CommentCreateFilmDTO
    {
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;

        public int? FilmId { get; set; }
    }
}
