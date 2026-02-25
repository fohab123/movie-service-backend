using movie_service_backend.DTO.WatchlistDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Models;
using movie_service_backend.Repo;

namespace movie_service_backend.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly WatchlistRepo _repo;

        public WatchlistService(WatchlistRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<WatchlistItemDTO>> GetByUserIdAsync(int userId)
        {
            var items = await _repo.GetByUserIdAsync(userId);
            return items.Select(MapToDTO);
        }

        public async Task<WatchlistItemDTO> AddAsync(WatchlistAddDTO dto)
        {
            if (await _repo.ExistsAsync(dto.UserId, dto.FilmId))
                throw new InvalidOperationException("Film is already in your watchlist.");

            var item = new WatchlistItem
            {
                UserId = dto.UserId,
                FilmId = dto.FilmId,
                Watched = false,
                AddedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(item);
            await _repo.SaveChangesAsync();

            var saved = await _repo.GetByIdAsync(item.Id);
            return MapToDTO(saved!);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return false;

            _repo.Remove(item);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<WatchlistItemDTO?> MarkWatchedAsync(int id, bool watched)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return null;

            item.Watched = watched;
            await _repo.SaveChangesAsync();

            return MapToDTO(item);
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static WatchlistItemDTO MapToDTO(WatchlistItem item)
        {
            var film = item.Film;

            double rating = film.Ratings != null && film.Ratings.Any()
                ? Math.Round(film.Ratings.Average(r => r.Value), 1)
                : 0;

            int voteCount = film.Ratings?.Count ?? 0;

            return new WatchlistItemDTO
            {
                Id = item.Id,
                FilmId = film.Id,
                Title = film.Title,
                Year = film.Year,
                Duration = FormatDuration(film.Duration),
                Rating = rating,
                VotesText = FormatVotes(voteCount),
                Description = film.Description,
                Director = film.Director,
                Stars = new List<string>(),
                PosterUrl = film.PosterUrl,
                Watched = item.Watched,
                AddedAt = item.AddedAt.ToString("o"),
                Genres = film.Genre?.Select(g => g.Name).ToList() ?? new List<string>()
            };
        }

        private static string FormatDuration(int minutes)
        {
            if (minutes <= 0) return "";
            int h = minutes / 60;
            int m = minutes % 60;
            return h > 0 ? $"{h}h {m}m" : $"{m}m";
        }

        private static string FormatVotes(int count)
        {
            if (count >= 1_000_000)
                return $"{count / 1_000_000.0:0.#}M";
            if (count >= 1_000)
                return $"{count / 1_000.0:0.#}K";
            return count.ToString();
        }
    }
}
