using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.DTO.SeriesDTOs;

namespace movie_service_backend.Interfaces
{
    public interface ISeriesService
    {
        Task<IEnumerable<SeriesDTO>> GetAllSeriesAsync();
        Task<SeriesDTO?> GetSeriesByIdAsync(int id);
        Task<SeriesDTO> CreateSeriesAsync(SeriesCreateDTO dto);
        Task<SeriesDTO?> UpdateSeriesAsync(int id, SeriesUpdateDTO dto);
        Task<bool> DeleteSeriesAsync(int id);
        Task<IEnumerable<object>> GetSeriesGroupedByGenreAsync();
        Task<IEnumerable<object>> GetAllSortedByDateAsync();
        Task<RecommendedSeriesDTO?> GetRecommendationAsync(int userId);
        Task<IEnumerable<SeriesDTO>> GetTrendingSeriesAsync();
    }
}
