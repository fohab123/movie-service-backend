using movie_service_backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Rating
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; }

    // Veza može biti ili FilmId ili SeriesId, zavisi šta ocenjujemo
    public int? FilmId { get; set; }
    public Film Film { get; set; }

    public int? SeriesId { get; set; }
    public Series Series { get; set; }

    [Range(1, 10)]
    public int Value { get; set; }
}
