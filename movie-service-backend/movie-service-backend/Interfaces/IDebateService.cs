using movie_service_backend.DTO.DebatePostDTOs;

namespace movie_service_backend.Interfaces
{
    public interface IDebateService
    {
        Task<DebatePostDTO> CreatePostAsync(DebatePostCreateDTO dto, int userId);
        Task<IEnumerable<DebatePostDTO>> GetAllRootPostsAsync();
        Task<DebatePostDTO?> GetDebateThreadAsync(int postId);
        Task<bool> DeletePostAsync(int postId);
        Task ToggleLikeAsync(int postId, int userId);
        Task<int> GetLikesCountAsync(int postId);
    }
}
