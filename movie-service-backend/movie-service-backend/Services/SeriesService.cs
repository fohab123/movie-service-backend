using AutoMapper;
using Microsoft.Identity.Client;
using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.DTO.SeriesDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Repo;

namespace movie_service_backend.Services
{
    public class SeriesService : ISeriesService
    {
        private readonly IMapper _mapper;
        private readonly SeriesRepo _repo;

        public SeriesService(IMapper mapper, SeriesRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        public async Task<SeriesDTO> CreateSeriesAsync(SeriesCreateDTO dto)
        {
            var series = _mapper.Map<Series>(dto);
            if (dto.GenreId != null && dto.GenreId.Any())
            {
                var genres = await _repo.GetGenresByIdsAsync(dto.GenreId);
                series.Genre = genres;
            }
            await _repo.AddAsync(series);
            await _repo.SaveChangesAsync();
            return _mapper.Map<SeriesDTO>(series);
        }

        public async Task<bool> DeleteSeriesAsync(int id)
        {
            var series = await _repo.GetByIdAsync(id);
            if (series == null) return false;
            _repo.Delete(series);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SeriesDTO>> GetAllSeriesAsync()
        {
            var series = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<SeriesDTO>>(series);
        }

        public async Task<SeriesDTO?> GetSeriesByIdAsync(int id)
        {
            var series = await _repo.GetByIdAsync(id);
            return series == null ? null : _mapper.Map<SeriesDTO>(series);
        }

        public async Task<SeriesDTO?> UpdateSeriesAsync(int id, SeriesUpdateDTO dto)
        {
            var series = await _repo.GetByIdAsync(id);
            if(series == null) return null;
            series.Title = dto.Title;
            series.Description = dto.Description;
            series.Year = dto.Year;
            if (dto.GenreId != null && dto.GenreId.Any())
            {
                var genres = await _repo.GetGenresByIdsAsync(dto.GenreId);
                series.Genre = genres;
            }
            series.Director = dto.Director;
            series.Seasons = dto.Seasons;
            series.PosterUrl = dto.PosterUrl;
            _repo.Update(series);
            await _repo.SaveChangesAsync();
            return _mapper.Map<SeriesDTO>(series);
        }
        public async Task<IEnumerable<object>> GetSeriesGroupedByGenreAsync()
        {
            var series = await _repo.GetAllAsync();
            var seriesDTOs = _mapper.Map<List<SeriesDTO>>(series);

            var grouped = seriesDTOs.GroupBy(f => f.Genre.Id).Select(g => new SeriesGenreGroupDTO
            {
                Genre = g.First().Genre,
                Series = g.ToList()
            }).ToList();
            return grouped;
        }
        public async Task<IEnumerable<object>> GetAllSortedByDateAsync()
        {
            var films = await _repo.GetAllSortedByDateAsync();
            return _mapper.Map<List<SeriesDTO>>(films);

        }
    }
}
