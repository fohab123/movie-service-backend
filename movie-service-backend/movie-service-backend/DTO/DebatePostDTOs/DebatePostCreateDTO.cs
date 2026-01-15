namespace movie_service_backend.DTO.DebatePostDTOs
{
    public class DebatePostCreateDTO
    {
        public string? Title { get; set; } // only for root posts
        public string Content { get; set; }
        public int? ParentId { get; set; } // null => root post
    }
}
