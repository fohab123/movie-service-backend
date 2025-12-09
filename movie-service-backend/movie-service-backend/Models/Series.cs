using movie_service_backend.Models;
using System.ComponentModel.DataAnnotations;

public class Series
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    public string Description { get; set; }

    public int Year { get; set; }

    public ICollection<Genre> Genre { get; set; } = new List<Genre>();

    [MaxLength(100)]
    public string Director { get; set; }

    public int Seasons { get; set; }

    public string PosterUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigacione property
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
