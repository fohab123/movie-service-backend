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
            var films = await _repo.GetAllAsync();
            var filmDTOs = _mapper.Map<List<FilmDTO>>(films);

            // Flatten: za svaki film, za svaki njegov žanr, napravi "par"
            var genreFilmPairs = filmDTOs
                .SelectMany(f => f.Genres, (f, g) => new { Film = f, Genre = g });

            // Grupisanje po žanru
            var grouped = genreFilmPairs
                .GroupBy(x => x.Genre.Id)
                .Select(g => new FilmGenreGroupDTO
                {
                    Genre = g.First().Genre,
                    Films = g.Select(x => x.Film).ToList()
                })
                .ToList();

            return grouped;
        }

        public async Task<IEnumerable<object>> GetAllSortedByDateAsync()
        {
            var films = await _repo.GetAllSortedByDateAsync();
            return _mapper.Map<List<FilmDTO>>(films);

        }


        public async Task<RecommendedFilmDTO?> GetRecommendationAsync(int userId)
        {
            // 1. Uzmi sve ocene korisnika (sa filmovima i žanrovima)
            var userRatings = await _repo.GetUserRatingsWithGenresAsync(userId);

            if (!userRatings.Any())
                return null;

            // 2. Izračunaj prosečan score po žanru od strane korisnika
            var genreScores = userRatings
                .SelectMany(r => r.Film.Genre.Select(g => new { GenreId = g.Id, r.Value }))
                .GroupBy(x => x.GenreId)
                .Select(g => new
                {
                    GenreId = g.Key,
                    AvgScore = g.Average(x => x.Value)
                })
                .OrderByDescending(x => x.AvgScore)
                .ToList();

            // 3. Uzmi sve filmove sa ocenama
            var films = await _repo.GetAllFilmsWithRatingsAsync();

            // 4. Izračunaj GenreScore + GlobalRating
            var scoredFilms = films.Select(f => new
            {
                Film = f,

                GenreScore = f.Genre
                    .Where(g => genreScores.Any(gs => gs.GenreId == g.Id))
                    .Sum(g => genreScores.First(gs => gs.GenreId == g.Id).AvgScore),

                GlobalRating = f.Ratings.Any()
                    ? f.Ratings.Average(r => r.Value)
                    : 0
            }).ToList();

            // Ako nijedan film nema score > 0, nema smisla preporuka
            double maxGenreScore = scoredFilms.Max(x => x.GenreScore);

            var bestFilm = scoredFilms
                .Where(x => x.GenreScore == maxGenreScore)
                .OrderByDescending(x => x.GlobalRating)
                .FirstOrDefault();

            if (bestFilm == null)
                return null;

            // 5. Mapiranje u DTO
            return new RecommendedFilmDTO
            {
                FilmId = bestFilm.Film.Id,
                Title = bestFilm.Film.Title,
                PosterUrl = bestFilm.Film.PosterUrl,
                Score = bestFilm.GenreScore,
                GlobalRating = bestFilm.GlobalRating
            };
        }
    }
}
