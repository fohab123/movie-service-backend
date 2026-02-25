using movie_service_backend.DTO.RatingDTOs;

namespace movie_service_backend.Interfaces
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingDTO>> GetAllAsync();
        Task<RatingDTO?> GetByIdAsync(int id);
        Task<RatingDTO> CreateForFilmAsync(RatingCreateFilmDTO dto);
        Task<RatingDTO> CreateForSeriesAsync(RatingCreateSeriesDTO dto);
        Task<RatingDTO> UpdateAsync(int id, RatingUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<RatingDTO>> GetByFilmIdAsync(int filmId);
        Task<IEnumerable<RatingDTO>> GetBySeriesIdAsync(int seriesId);
        Task<RatingDTO?> GetUserFilmRatingAsync(int userId, int filmId);
        Task<RatingDTO?> GetUserSeriesRatingAsync(int userId, int seriesId);
    }
}
