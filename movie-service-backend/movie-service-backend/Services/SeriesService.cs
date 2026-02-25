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
            var series = await _repo.GetAllSeriesWithRatingsAsync();
            var seriesDTOs = _mapper.Map<List<SeriesDTO>>(series);

            var genreSeriesPairs = seriesDTOs
                .SelectMany(s => s.Genres, (s, g) => new { Series = s, Genre = g });

            var grouped = genreSeriesPairs
                .GroupBy(x => x.Genre.Id)
                .Select(g => new SeriesGenreGroupDTO
                {
                    Genre = g.First().Genre,
                    Series = g.Select(x => x.Series)
                              .OrderByDescending(s => s.Rating ?? 0)
                              .ToList()
                })
                .ToList();

            return grouped;
        }
        public async Task<IEnumerable<object>> GetAllSortedByDateAsync()
        {
            var films = await _repo.GetAllSortedByDateAsync();
            return _mapper.Map<List<SeriesDTO>>(films);

        }

        public async Task<IEnumerable<RecommendedSeriesDTO>> GetRecommendationAsync(int userId)
        {
            var userRatings = await _repo.GetUserRatingsWithGenresAsync(userId);
            if (!userRatings.Any())
                return Enumerable.Empty<RecommendedSeriesDTO>();

            var genreScores = userRatings
                .SelectMany(r => r.Series.Genre.Select(g => new { GenreId = g.Id, r.Value }))
                .GroupBy(x => x.GenreId)
                .Select(g => new
                {
                    GenreId = g.Key,
                    AvgScore = g.Average(x => x.Value)
                })
                .ToList();

            var series = await _repo.GetAllSeriesWithRatingsAsync();

            var recommendations = series.Select(s => new
            {
                Series = s,
                GenreScore = s.Genre
                    .Where(g => genreScores.Any(gs => gs.GenreId == g.Id))
                    .Sum(g => genreScores.First(gs => gs.GenreId == g.Id).AvgScore),
                GlobalRating = s.Ratings.Any()
                    ? s.Ratings.Average(r => r.Value)
                    : 0
            })
            .Where(x => x.GenreScore > 0)
            .OrderByDescending(x => x.GenreScore)
            .ThenByDescending(x => x.GlobalRating)
            .Take(10)
            .Select(x => new RecommendedSeriesDTO
            {
                SeriesId = x.Series.Id,
                Title = x.Series.Title,
                PosterUrl = x.Series.PosterUrl,
                Score = x.GenreScore,
                GlobalRating = x.GlobalRating
            })
            .ToList();

            return recommendations;
        }

        public async Task<IEnumerable<SeriesDTO>> GetTrendingSeriesAsync()
        {
            var series = await _repo.GetAllWithRatingsCommentsAsync();

            var sinceDate = DateTime.UtcNow.AddDays(-30);

            var trending = series.
                Select(s => new
                {
                    Series = s,
                    RecentRatings = s.Ratings.Where(r => r.CreatedAt >= sinceDate)
                    .ToList()
                })
                .Where(x => x.RecentRatings.Any())
                .Select(x => new
                {
                    x.Series,
                    AvgRating = x.RecentRatings.Average(r => r.Value)
                })
                .OrderByDescending(x => x.AvgRating)
                .Take(10)
                .Select(x => x.Series)
                .ToList();

            return _mapper.Map<IEnumerable<SeriesDTO>>(trending);
        }
    }
}
