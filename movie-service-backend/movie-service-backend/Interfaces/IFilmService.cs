using movie_service_backend.DTO.FilmDTOs;

namespace movie_service_backend.Interfaces
{
    public interface IFilmService
    {
        Task<IEnumerable<FilmDTO>> GetAllAsync();
        Task<FilmDTO?> GetByIdAsync(int id);
        Task<FilmDTO> CreateAsync(FilmCreateDTO dto);
        Task<FilmDTO?> UpdateAsync(int id, FilmCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
