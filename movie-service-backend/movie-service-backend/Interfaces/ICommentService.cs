using movie_service_backend.DTO.CommentDTOs;
using movie_service_backend.DTO.RatingDTOs;

namespace movie_service_backend.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetAllAsync();
        Task<CommentDTO?> GetByIdAsync(int id);
        Task<CommentDTO> CreateForFilmAsync(CommentCreateFilmDTO dto);
        Task<CommentDTO> CreateForSeriesAsync(CommentCreateSeriesDTO dto);
        Task<CommentDTO> UpdateAsync(int id, CommentUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CommentDTO>> GetByFilmIdAsync(int filmId);
        Task<IEnumerable<CommentDTO>> GetBySeriesIdAsync(int seriesId);
    }
}
