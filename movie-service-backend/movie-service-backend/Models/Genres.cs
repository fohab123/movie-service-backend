using System.ComponentModel.DataAnnotations;

namespace movie_service_backend.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Navigaciona property za filmove koji imaju ovaj žanr
        public ICollection<Film> Films { get; set; }
        public ICollection<Series> Series { get; set; }
    }
}
