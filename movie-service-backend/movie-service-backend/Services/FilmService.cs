using AutoMapper;
using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Models;

namespace movie_service_backend.Services
{
    public class FilmService : IFilmService
    {
        private readonly IMapper _mapper;
        private readonly IRepo<Film> _repo;

        public FilmService(IMapper mapper, IRepo<Film> repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<FilmDTO> CreateAsync(FilmCreateDTO dto)
        {
            var film = _mapper.Map<Film>(dto);
            await _repo.AddAsync(film);
            await _repo.SaveChangesAsync();
            return _mapper.Map<FilmDTO>(film);
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FilmDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FilmDTO?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<FilmDTO?> UpdateAsync(int id, FilmCreateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
