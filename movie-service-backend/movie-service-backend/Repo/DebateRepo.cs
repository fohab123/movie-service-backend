using Microsoft.EntityFrameworkCore;
using movie_service_backend.Data;
using movie_service_backend.DTO.DebatePostDTOs;
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
                .Include(p=>p.User)
                .Include(l=>l.Likes)
                .OrderBy(p=>p.CreatedAt)
                .ToListAsync();
        }

        public async Task<DebatePost?> GetByIdAsync(int id)
        {
            return await _context.DebatePosts
                .Include(p => p.User)
                .Include(l=>l.Likes)
                .Include(r=>r.Replies)
                .ThenInclude(r=>r.Likes)
                .Include(r => r.Replies)
                .ThenInclude(r=>r.User)
                .FirstOrDefaultAsync(p => p.Id == id);
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
