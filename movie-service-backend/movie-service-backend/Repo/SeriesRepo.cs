using movie_service_backend.Data;
using movie_service_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using movie_service_backend.Models;

namespace movie_service_backend.Repo
{
    public class SeriesRepo : IRepo<Series>
    {
        private readonly AppDbContext _context;

        public SeriesRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task AddAsync(Series entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(Series entity)
        {
            _context.Series.Remove(entity);
        }

        public async Task<IEnumerable<Series>> GetAllAsync()
        {
            return await _context.Series
            .Include(f => f.Genre)   // <-- uključuje i žanr
            .ToListAsync();
        }

        public async Task<Series?> GetByIdAsync(int id)
        {
            return await _context.Series.Include(f => f.Genre).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Series entity)
        {
            _context.Series.Update(entity);
        }
        public async Task<IEnumerable<Series>> GetAllSortedByDateAsync()
        {
            return await _context.Series
                .Include(f => f.Genre)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }
        public async Task<List<Genre>> GetGenresByIdsAsync(List<int> ids)
        {
            return await _context.Genres
                .Where(g => ids.Contains(g.Id))
                .ToListAsync();
        }
    }
}
