using movie_service_backend.DTO.WatchlistDTOs;

namespace movie_service_backend.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<WatchlistItemDTO>> GetByUserIdAsync(int userId);
        Task<WatchlistItemDTO> AddAsync(WatchlistAddDTO dto);
        Task<bool> RemoveAsync(int id);
        Task<WatchlistItemDTO?> MarkWatchedAsync(int id, bool watched);
    }
}
