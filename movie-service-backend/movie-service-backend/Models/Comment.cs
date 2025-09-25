using movie_service_backend.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; }

    public int? FilmId { get; set; }
    public Film Film { get; set; }

    public int? SeriesId { get; set; }
    public Series Series { get; set; }

    [Required]
    public string Text { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
