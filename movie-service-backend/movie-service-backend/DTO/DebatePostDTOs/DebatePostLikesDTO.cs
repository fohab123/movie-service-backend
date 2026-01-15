namespace movie_service_backend.DTO.DebatePostDTOs
{
    public class DebatePostLikesDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
