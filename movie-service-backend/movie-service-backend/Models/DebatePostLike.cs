namespace movie_service_backend.Models
{
    public class DebatePostLike
    {
        public int Id { get; set; }

        public int DebatePostId { get; set; }
        public DebatePost DebatePost { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
