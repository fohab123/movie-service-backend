using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movie_service_backend.Models
{
    public class DebatePost
    {
        [Key]
        public int Id { get; set; }

        // Root posts mogu imati naslov; replies obično nemaju
        [MaxLength(300)]
        public string? Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Author
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        // Self reference: parent post (null => root)
        public int? ParentId { get; set; }
        public DebatePost? Parent { get; set; }

        // Link to Film or Series
        public int? FilmId { get; set; }
        public Film? Film { get; set; }

        public int? SeriesId { get; set; }
        public Series? Series { get; set; }

        // Comma-separated predefined tags e.g. "Spoiler,Theory"
        public string? Tags { get; set; }

        public bool IsSpoiler { get; set; } = false;

        public int ViewCount { get; set; } = 0;

        // Navigation: replies (children)
        public ICollection<DebatePost> Replies { get; set; } = new List<DebatePost>();
        public ICollection<DebatePostLike> Likes { get; set; } = new List<DebatePostLike>();

    }
}
