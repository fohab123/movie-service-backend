using movie_service_backend.DTO.DebatePostDTOs;

namespace movie_service_backend.Interfaces
{
    public interface IDebateService
    {
        Task<DebatePostDTO> CreatePostAsync(DebatePostCreateDTO dto, int userId);
        Task<IEnumerable<DebatePostDTO>> GetAllAsync(string? sort, int? filmId, int? seriesId, int? userId);
        Task<IEnumerable<DebatePostDTO>> GetAllRootPostsAsync();
        Task<DebatePostDTO?> GetDebateThreadAsync(int postId, int? userId);
        Task<bool> DeletePostAsync(int postId);
        Task ToggleLikeAsync(int postId, int userId);
        Task<int> GetLikesCountAsync(int postId);
        Task IncrementViewAsync(int postId);
        Task<IEnumerable<DebatePostDTO>> GetByFilmAsync(int filmId, int? userId);
        Task<IEnumerable<DebatePostDTO>> GetBySeriesAsync(int seriesId, int? userId);
    }
}
