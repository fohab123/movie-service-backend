using Microsoft.EntityFrameworkCore;
using movie_service_backend.Data;
using movie_service_backend.Interfaces;
using movie_service_backend.Models;

namespace movie_service_backend.Repo
{
    public class DebateRepo : IRepo<DebatePost>
    {
        private readonly AppDbContext _context;
        public DebateRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DebatePost entity)
        {
            await _context.DebatePosts.AddAsync(entity);
        }

        public void Delete(DebatePost entity)
        {
            _context.DebatePosts.Remove(entity);
        }

        public async Task<IEnumerable<DebatePost>> GetAllAsync()
        {
            return await _context.DebatePosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Film)
                .Include(p => p.Series)
                .Include(p => p.Replies)
                .OrderByDescending(p => p.Likes.Count)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DebatePost>> GetRootPostsFilteredAsync(int? filmId, int? seriesId)
        {
            var query = _context.DebatePosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Film)
                .Include(p => p.Series)
                .Include(p => p.Replies)
                .Where(p => p.ParentId == null)
                .AsQueryable();

            if (filmId.HasValue)
                query = query.Where(p => p.FilmId == filmId.Value);

            if (seriesId.HasValue)
                query = query.Where(p => p.SeriesId == seriesId.Value);

            return await query.ToListAsync();
        }

        public async Task<DebatePost?> GetByIdAsync(int id)
        {
            return await _context.DebatePosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Film)
                .Include(p => p.Series)
                // Level 1 replies
                .Include(p => p.Replies)
                    .ThenInclude(r => r.User)
                .Include(p => p.Replies)
                    .ThenInclude(r => r.Likes)
                // Level 2 replies
                .Include(p => p.Replies)
                    .ThenInclude(r => r.Replies)
                        .ThenInclude(r2 => r2.User)
                .Include(p => p.Replies)
                    .ThenInclude(r => r.Replies)
                        .ThenInclude(r2 => r2.Likes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<DebatePost>> GetByFilmIdAsync(int filmId)
        {
            return await _context.DebatePosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Film)
                .Include(p => p.Replies)
                .Where(p => p.FilmId == filmId && p.ParentId == null)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DebatePost>> GetBySeriesIdAsync(int seriesId)
        {
            return await _context.DebatePosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Series)
                .Include(p => p.Replies)
                .Where(p => p.SeriesId == seriesId && p.ParentId == null)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(DebatePost entity)
        {
            _context.DebatePosts.Update(entity);
        }

        public async Task<IEnumerable<DebatePost>> GetRepliesSortedAsync(int postId)
        {
            return await _context.DebatePosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Where(p => p.ParentId == postId)
                .OrderByDescending(p => p.Likes.Count)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
