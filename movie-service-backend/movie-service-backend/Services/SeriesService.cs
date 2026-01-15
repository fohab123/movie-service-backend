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

        public async Task<RecommendedSeriesDTO?> GetRecommendationAsync(int userId)
        {
            var userRatings = await _repo.GetUserRatingsWithGenresAsync(userId);
            if (!userRatings.Any())
                return null;
            var genreScores = userRatings
                .SelectMany(r => r.Series.Genre.Select(g => new { GenreId = g.Id, r.Value }))
                .GroupBy(x => x.GenreId)
                .Select(g => new
                {
                    GenreId = g.Key,
                    AvgScore = g.Average(x => x.Value)
                })
                .OrderByDescending(x => x.AvgScore)
                .ToList();

            var series = await _repo.GetAllSeriesWithRatingsAsync();

            var scoredSeries = series.Select(s => new
            {
                Series = s,

                GenreScore = s.Genre
                    .Where(g => genreScores.Any(gs => gs.GenreId == g.Id))
                    .Sum(g => genreScores.First(gs => gs.GenreId == g.Id).AvgScore),

                GlobalRating = s.Ratings.Any()
                    ? s.Ratings.Average(r => r.Value)
                    : 0
            }).ToList();

            double maxGenreScore = scoredSeries.Max(x => x.GenreScore);

            var bestSeries = scoredSeries
                .Where(x => x.GenreScore == maxGenreScore)
                .OrderByDescending(x => x.GlobalRating)
                .FirstOrDefault();
            if (bestSeries == null)
                return null;

            return new RecommendedSeriesDTO
            {
                SeriesId = bestSeries.Series.Id,
                Title = bestSeries.Series.Title,
                PosterUrl = bestSeries.Series.PosterUrl,
                Score = bestSeries.GenreScore,
                GlobalRating = bestSeries.GlobalRating
            };
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
