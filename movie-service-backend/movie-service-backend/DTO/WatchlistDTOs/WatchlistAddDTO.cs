using System.ComponentModel.DataAnnotations;

namespace movie_service_backend.DTO.WatchlistDTOs
{
    public class WatchlistAddDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int FilmId { get; set; }
    }
}
