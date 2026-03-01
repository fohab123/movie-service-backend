namespace movie_service_backend.DTO.DebatePostDTOs
{
    public class DebatePostCreateDTO
    {
        public string? Title { get; set; }
        public string Content { get; set; }
        public int? ParentId { get; set; }

        public int? FilmId { get; set; }
        public int? SeriesId { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
        public bool IsSpoiler { get; set; } = false;
    }
}
