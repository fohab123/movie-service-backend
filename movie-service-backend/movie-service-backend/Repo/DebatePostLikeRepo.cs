using Microsoft.EntityFrameworkCore;
using movie_service_backend.Data;
using movie_service_backend.DTO.DebatePostDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Models;

namespace movie_service_backend.Repo
{
    public class DebatePostLikeRepo : IRepo<DebatePostLike>
    {
        private readonly AppDbContext _context;
        public DebatePostLikeRepo(AppDbContext context)
        {
            _context = context ?? throw new Exception("AppDbContext is null");
        }
        public async Task AddAsync(DebatePostLike entity)
        {
            await _context.DebatePostLikes.AddAsync(entity);
        }

        public void Delete(DebatePostLike entity)
        {
            _context.DebatePostLikes.Remove(entity);
        }

        public async Task<IEnumerable<DebatePostLike>> GetAllAsync()
        {
            return await _context.DebatePostLikes
                .Include(l => l.User)
                .Include(l => l.DebatePost)
                .ToListAsync();
        }

        public async Task<DebatePostLike?> GetByIdAsync(int id)
        {
            return await _context.DebatePostLikes
                .Include(l => l.User)
                .Include(l => l.DebatePost)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(DebatePostLike entity)
        {
            _context.DebatePostLikes.Update(entity);
        }
        public async Task<DebatePostLike?> GetByPostAndUserAsync(int postId, int userId)
        {
            return await _context.DebatePostLikes
                .FirstOrDefaultAsync(l =>
                    l.DebatePostId == postId &&
                    l.UserId == userId);
        }

        public async Task<int> CountByPostAsync(int postId)
        {
            return await _context.DebatePostLikes
                .CountAsync(l => l.DebatePostId == postId);
        }
    }
}
