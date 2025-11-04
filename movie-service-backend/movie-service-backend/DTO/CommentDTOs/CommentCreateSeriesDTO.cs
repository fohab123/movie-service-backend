namespace movie_service_backend.DTO.CommentDTOs
{
    public class CommentCreateSeriesDTO
    {
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;

        public int? SeriesId { get; set; }
    }
}
