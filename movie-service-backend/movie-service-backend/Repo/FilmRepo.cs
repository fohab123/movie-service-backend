using movie_service_backend.Data;
using movie_service_backend.Interfaces;
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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Film>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Film?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Film entity)
        {
            throw new NotImplementedException();
        }
    }
}
