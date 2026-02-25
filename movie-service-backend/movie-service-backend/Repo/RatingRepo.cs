using Microsoft.EntityFrameworkCore;
using movie_service_backend.Data;
using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Models;

namespace movie_service_backend.Repo
{
    public class RatingRepo : IRepo<Rating>
    {
        private readonly AppDbContext _context;

        public RatingRepo(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Rating entity)
        {
            await _context.Ratings.AddAsync(entity);
        }

        public void Delete(Rating entity)
        {
            _context.Ratings.Remove(entity);
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            return await _context.Ratings.
                Include(r => r.User).
                Include(r => r.Film).
                Include(r => r.Series).
                ToListAsync();
        }

        public async Task<Rating?> GetByIdAsync(int id)
        {
            return await _context.Ratings.
                Include(r => r.User).
                Include(r => r.Film).
                Include(r => r.Series).
                FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Rating entity)
        {
            _context.Ratings.Update(entity);
        }

        public async Task<Rating?> GetUserFilmRatingAsync(int userId, int filmId)
        {
            return await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.FilmId == filmId);
        }

        public async Task<Rating?> GetUserSeriesRatingAsync(int userId, int seriesId)
        {
            return await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.SeriesId == seriesId);
        }

        public async Task<IEnumerable<Rating>> GetByFilmIdAsync(int filmId)
        {
            return await _context.Ratings
                .Where(r => r.FilmId == filmId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetBySeriesIdAsync(int seriesId)
        {
            return await _context.Ratings
                .Where(r => r.SeriesId == seriesId)
                .ToListAsync();
        }
    }
}
