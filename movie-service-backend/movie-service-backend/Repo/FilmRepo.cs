using movie_service_backend.Data;
using movie_service_backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using movie_service_backend.Models;

namespace movie_service_backend.Repo
{
    public class FilmRepo : IRepo<Film>
    {
        private readonly AppDbContext _context;

        public FilmRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Film entity)
        {
            await _context.Films.AddAsync(entity);
        }

        public void Delete(Film entity)
        {
            _context.Films.Remove(entity);
        }

        public async Task<IEnumerable<Film>> GetAllAsync()
        {
             return await _context.Films
             .Include(f => f.Genre)   // <-- uključuje i žanr
             .ToListAsync();
        }

        public async Task<Film?> GetByIdAsync(int id)
        {
            return await _context.Films
           .Include(f => f.Genre)   // <-- da imaš i žanr u GetById
           .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Film entity)
        {
            _context.Films.Update(entity);
        }
        public async Task<IEnumerable<Film>> GetAllSortedByDateAsync()
        {
            return await _context.Films
                .Include(f => f.Genre)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }
    }
}
