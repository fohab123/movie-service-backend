using Microsoft.EntityFrameworkCore;
using movie_service_backend.Data;
using movie_service_backend.Models;

namespace movie_service_backend.Repo
{
    public class WatchlistRepo
    {
        private readonly AppDbContext _context;

        public WatchlistRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WatchlistItem>> GetByUserIdAsync(int userId)
        {
            return await _context.WatchlistItems
                .Where(w => w.UserId == userId)
                .Include(w => w.Film)
                    .ThenInclude(f => f.Genre)
                .Include(w => w.Film)
                    .ThenInclude(f => f.Ratings)
                .OrderBy(w => w.AddedAt)
                .ToListAsync();
        }

        public async Task<WatchlistItem?> GetByIdAsync(int id)
        {
            return await _context.WatchlistItems
                .Include(w => w.Film)
                    .ThenInclude(f => f.Genre)
                .Include(w => w.Film)
                    .ThenInclude(f => f.Ratings)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<bool> ExistsAsync(int userId, int filmId)
        {
            return await _context.WatchlistItems
                .AnyAsync(w => w.UserId == userId && w.FilmId == filmId);
        }

        public async Task AddAsync(WatchlistItem item)
        {
            await _context.WatchlistItems.AddAsync(item);
        }

        public void Remove(WatchlistItem item)
        {
            _context.WatchlistItems.Remove(item);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
