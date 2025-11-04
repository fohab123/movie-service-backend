using movie_service_backend.DTO.FilmDTOs;

namespace movie_service_backend.Interfaces
{
    public interface IFilmService
    {
        Task<IEnumerable<FilmDTO>> GetAllFilmsAsync();
        Task<FilmDTO?> GetFilmByIdAsync(int id);
        Task<FilmDTO> CreateFilmAsync(FilmCreateDTO dto);
        Task<FilmDTO?> UpdateFilmAsync(int id, FilmCreateDTO dto);
        Task<bool> DeleteFilmAsync(int id);
        Task<IEnumerable<object>> GetFilmsGroupedByGenreAsync();
        Task<IEnumerable<object>> GetAllSortedByDateAsync();
    }
}
