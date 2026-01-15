namespace movie_service_backend.DTO.DebatePostDTOs
{
    public class DebatePostDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; } // null for replies
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public string? Username { get; set; }
        public int LikesCount { get; set; }

        public List<DebatePostDTO> Replies { get; set; } = new List<DebatePostDTO>();
    }
}
