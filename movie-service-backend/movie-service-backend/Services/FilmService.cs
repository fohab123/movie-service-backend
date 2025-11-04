using AutoMapper;
using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Models;
using movie_service_backend.Repo;

namespace movie_service_backend.Services
{
    public class FilmService : IFilmService
    {
        private readonly IMapper _mapper;
        private readonly FilmRepo _repo;

        public FilmService(IMapper mapper, FilmRepo filmRepo)
        {
            _mapper = mapper;
            _repo = filmRepo;
        }

        public async Task<FilmDTO> CreateFilmAsync(FilmCreateDTO dto)
        {
            var film = _mapper.Map<Film>(dto);
            film.GenreId = dto.GenreId;
            await _repo.AddAsync(film);
            await _repo.SaveChangesAsync();
            return _mapper.Map<FilmDTO>(film);
        }

        public async Task<bool> DeleteFilmAsync(int id)
        {
            var film = await _repo.GetByIdAsync(id);
            if (film == null) return false;
            _repo.Delete(film);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FilmDTO>> GetAllFilmsAsync()
        {
            var films = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<FilmDTO>>(films);
        }

        public async Task<FilmDTO?> GetFilmByIdAsync(int id)
        {
            var film = await _repo.GetByIdAsync(id);
            return film == null ? null : _mapper.Map<FilmDTO>(film);
        }

        public async Task<FilmDTO> UpdateFilmAsync(int id, FilmCreateDTO dto)
        {
            var film = await _repo.GetByIdAsync(id);
            if (film == null) return null;
            film.Title = dto.Title;
            film.Description = dto.Description;
            film.Year = dto.Year;
            film.GenreId = dto.GenreId;
            film.Director = dto.Director;
            film.Duration = dto.Duration;
            film.PosterUrl = dto.PosterUrl;
            _repo.Update(film);
            await _repo.SaveChangesAsync();
            return _mapper.Map<FilmDTO>(film);

        }

        public async Task<IEnumerable<object>> GetFilmsGroupedByGenreAsync()
        {
            var films = await _repo.GetAllAsync();
            var filmDTOs = _mapper.Map<List<FilmDTO>>(films);

            var grouped = filmDTOs.GroupBy(f => f.Genre.Id).Select(g => new FilmGenreGroupDTO
            {
                Genre = g.First().Genre,
                Films = g.ToList()
            }).ToList();
            return grouped;
        }
        public async Task<IEnumerable<object>> GetAllSortedByDateAsync()
        {
            var films = await _repo.GetAllSortedByDateAsync();
            return _mapper.Map<List<FilmDTO>>(films);

        }
    }
}
