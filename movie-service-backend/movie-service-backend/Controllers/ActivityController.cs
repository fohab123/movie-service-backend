using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_service_backend.Data;
using movie_service_backend.DTO.ActivityDTOs;

namespace movie_service_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ActivityController(AppDbContext db)
        {
            _db = db;
        }

        // GET /api/Activity/recent?limit=20
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int limit = 20)
        {
            var comments = await _db.Comments
                .Include(c => c.User)
                .Include(c => c.Film)
                .Include(c => c.Series)
                .OrderByDescending(c => c.CreatedAt)
                .Take(limit)
                .Select(c => new RecentActivityDTO
                {
                    Type = "comment",
                    Username = c.User.Username,
                    UserId = c.UserId,
                    MediaTitle = c.Film != null ? c.Film.Title : c.Series.Title,
                    MediaType = c.FilmId != null ? "film" : "series",
                    MediaId = c.FilmId != null ? c.FilmId.Value : c.SeriesId.Value,
                    MediaPosterUrl = c.Film != null ? c.Film.PosterUrl : c.Series.PosterUrl,
                    Text = c.Text,
                    Value = null,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            var ratings = await _db.Ratings
                .Include(r => r.User)
                .Include(r => r.Film)
                .Include(r => r.Series)
                .OrderByDescending(r => r.CreatedAt)
                .Take(limit)
                .Select(r => new RecentActivityDTO
                {
                    Type = "rating",
                    Username = r.User.Username,
                    UserId = r.UserId,
                    MediaTitle = r.Film != null ? r.Film.Title : r.Series.Title,
                    MediaType = r.FilmId != null ? "film" : "series",
                    MediaId = r.FilmId != null ? r.FilmId.Value : r.SeriesId.Value,
                    MediaPosterUrl = r.Film != null ? r.Film.PosterUrl : r.Series.PosterUrl,
                    Text = null,
                    Value = r.Value,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            var result = comments
                .Concat(ratings)
                .OrderByDescending(a => a.CreatedAt)
                .Take(limit)
                .ToList();

            return Ok(result);
        }
    }
}
