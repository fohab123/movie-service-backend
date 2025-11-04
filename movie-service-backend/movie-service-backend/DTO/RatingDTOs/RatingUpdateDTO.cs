using System.ComponentModel.DataAnnotations;

namespace movie_service_backend.DTO.RatingDTOs
{
    public class RatingUpdateDTO
    {
        [Range(1, 10)]
        public int Value { get; set; }
    }
}
