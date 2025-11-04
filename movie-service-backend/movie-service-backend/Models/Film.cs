using movie_service_backend.Models;
using System.ComponentModel.DataAnnotations;

public class Film
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    public string Description { get; set; }

    public int Year { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; }

    [MaxLength(100)]
    public string Director { get; set; }

    public int Duration { get; set; } // u minutima

    public string PosterUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigacione property
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Comment> Comments { get; set; }
}