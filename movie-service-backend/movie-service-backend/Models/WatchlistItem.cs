using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movie_service_backend.Models
{
    public class WatchlistItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Film")]
        public int FilmId { get; set; }
        public Film Film { get; set; }

        public bool Watched { get; set; } = false;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
