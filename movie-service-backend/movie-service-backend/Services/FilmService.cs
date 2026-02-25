using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            // Mapiranje osnovnih polja
            var film = _mapper.Map<Film>(dto);

            // Dohvati žanrove preko repozitorijuma
            if (dto.GenreId != null && dto.GenreId.Any())
            {
                var genres = await _repo.GetGenresByIdsAsync(dto.GenreId);
                film.Genre = genres;
            }

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
            film.Director = dto.Director;
            film.Duration = dto.Duration;
            film.PosterUrl = dto.PosterUrl;

            // Ažuriraj žanrove
            if (dto.GenreId != null && dto.GenreId.Any())
            {
                var genres = await _repo.GetGenresByIdsAsync(dto.GenreId);
                film.Genre = genres;
            }

            _repo.Update(film);
            await _repo.SaveChangesAsync();

            return _mapper.Map<FilmDTO>(film);

        }

        public async Task<IEnumerable<FilmGenreGroupDTO>> GetFilmsGroupedByGenreAsync()
        {
            var films = await _repo.GetAllFilmsWithRatingsAsync();
            var filmDTOs = _mapper.Map<List<FilmDTO>>(films);

            var genreFilmPairs = filmDTOs
                .SelectMany(f => f.Genres, (f, g) => new { Film = f, Genre = g });

            var grouped = genreFilmPairs
                .GroupBy(x => x.Genre.Id)
                .Select(g => new FilmGenreGroupDTO
                {
                    Genre = g.First().Genre,
                    Films = g.Select(x => x.Film)
                             .OrderByDescending(f => f.Rating ?? 0)
                             .ToList()
                })
                .ToList();

            return grouped;
        }

        public async Task<IEnumerable<object>> GetAllSortedByDateAsync()
        {
            var films = await _repo.GetAllSortedByDateAsync();
            return _mapper.Map<List<FilmDTO>>(films);

        }


        public async Task<IEnumerable<RecommendedFilmDTO>> GetRecommendationAsync(int userId)
        {
            var userRatings = await _repo.GetUserRatingsWithGenresAsync(userId);

            if (!userRatings.Any())
                return Enumerable.Empty<RecommendedFilmDTO>();

            var genreScores = userRatings
                .SelectMany(r => r.Film.Genre.Select(g => new { GenreId = g.Id, r.Value }))
                .GroupBy(x => x.GenreId)
                .Select(g => new
                {
                    GenreId = g.Key,
                    AvgScore = g.Average(x => x.Value)
                })
                .ToList();

            var films = await _repo.GetAllFilmsWithRatingsAsync();

            var recommendations = films.Select(f => new
            {
                Film = f,
                GenreScore = f.Genre
                    .Where(g => genreScores.Any(gs => gs.GenreId == g.Id))
                    .Sum(g => genreScores.First(gs => gs.GenreId == g.Id).AvgScore),
                GlobalRating = f.Ratings.Any()
                    ? f.Ratings.Average(r => r.Value)
                    : 0
            })
            .Where(x => x.GenreScore > 0)
            .OrderByDescending(x => x.GenreScore)
            .ThenByDescending(x => x.GlobalRating)
            .Take(10)
            .Select(x => new RecommendedFilmDTO
            {
                FilmId = x.Film.Id,
                Title = x.Film.Title,
                PosterUrl = x.Film.PosterUrl,
                Score = x.GenreScore,
                GlobalRating = x.GlobalRating
            })
            .ToList();

            return recommendations;
        }

        public async Task<IEnumerable<FilmDTO>> GetTrendingFilmsAsync()
        {
            var films = await _repo.GetAllWithRatingsCommentsAsync();

            var sinceDate = DateTime.UtcNow.AddDays(-30);

            var trending = films.
                Select(f => new
                {
                    Film = f,
                    RecentRatings = f.Ratings.Where(r => r.CreatedAt >= sinceDate)
                    .ToList()
                })
                .Where(x => x.RecentRatings.Any())
                .Select(x => new
                {
                    x.Film,
                    AvgRating = x.RecentRatings.Average(r => r.Value)
                })
                .OrderByDescending(x => x.AvgRating)
                .Take(10)
                .Select(x => x.Film)
                .ToList();

            return _mapper.Map<IEnumerable<FilmDTO>>(trending);
        }
    }
}
