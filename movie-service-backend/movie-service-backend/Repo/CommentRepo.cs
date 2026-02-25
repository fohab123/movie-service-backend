using movie_service_backend.Data;
using movie_service_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using movie_service_backend.Models;

namespace movie_service_backend.Repo
{
    public class CommentRepo : IRepo<Comment>
    {
        private readonly AppDbContext _context;

        public CommentRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
        }

        public void Delete(Comment entity)
        {
            _context.Comments.Remove(entity);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Comment entity)
        {
            _context.Comments.Update(entity);
        }
        public async Task<Comment?> GetLastUserCommentAsync(int userId, int? filmId, int? seriesId)
        {
            return await _context.Comments
                .Where(c =>
                c.UserId == userId &&
                c.FilmId == filmId &&
                c.SeriesId == seriesId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Comment>> GetByFilmIdAsync(int filmId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.FilmId == filmId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetBySeriesIdAsync(int seriesId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.SeriesId == seriesId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
