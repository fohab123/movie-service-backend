using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_service_backend.Data;
using movie_service_backend.Models;
using System.Text.Json;

namespace movie_service_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly HttpClient _http;

        private static readonly Dictionary<string, int> MovieGenreMap = new()
        {
            { "Action", 28 },
            { "Adventure", 12 },
            { "Comedy", 35 },
            { "Crime & Mystery", 80 },
            { "Drama", 18 },
            { "Fantasy", 14 },
            { "Horror", 27 },
            { "Romance", 10749 },
            { "Sci-Fi", 878 },
            { "War", 10752 },
            { "Western", 37 }
        };

        private static readonly Dictionary<string, int> TvGenreMap = new()
        {
            { "Action", 10759 },
            { "Adventure", 10759 },
            { "Comedy", 35 },
            { "Crime & Mystery", 80 },
            { "Drama", 18 },
            { "Fantasy", 10765 },
            { "Horror", 9648 },
            { "Romance", 18 },
            { "Sci-Fi", 10765 },
            { "War", 10768 },
            { "Western", 37 }
        };

        public SeedController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _http = new HttpClient();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFromTmdb()
        {
            var apiKey = _config["Tmdb:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_TMDB_API_KEY_HERE")
                return BadRequest("Please set your TMDB API key in appsettings.json");

            // 1. Ensure genres exist
            var genres = new Dictionary<string, Genre>();
            foreach (var genreName in MovieGenreMap.Keys)
            {
                var existing = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (existing == null)
                {
                    existing = new Genre { Name = genreName };
                    _context.Genres.Add(existing);
                    await _context.SaveChangesAsync();
                }
                genres[genreName] = existing;
            }

            int filmsImported = 0;
            int seriesImported = 0;

            // 2. Import movies per genre
            foreach (var (genreName, tmdbGenreId) in MovieGenreMap)
            {
                var url = $"https://api.themoviedb.org/3/discover/movie?api_key={apiKey}&with_genres={tmdbGenreId}&sort_by=popularity.desc&page=1";
                var response = await _http.GetStringAsync(url);
                var json = JsonDocument.Parse(response);
                var results = json.RootElement.GetProperty("results");

                foreach (var movie in results.EnumerateArray().Take(20))
                {
                    var title = movie.GetProperty("title").GetString();
                    if (string.IsNullOrEmpty(title)) continue;

                    // Skip if already exists
                    var exists = await _context.Films.AnyAsync(f => f.Title == title);
                    if (exists) continue;

                    var tmdbId = movie.GetProperty("id").GetInt32();
                    var posterPath = movie.TryGetProperty("poster_path", out var pp) && pp.ValueKind != JsonValueKind.Null
                        ? pp.GetString() : null;
                    var overview = movie.TryGetProperty("overview", out var ov) ? ov.GetString() ?? "" : "";
                    var releaseDate = movie.TryGetProperty("release_date", out var rd) ? rd.GetString() ?? "" : "";
                    int year = 0;
                    if (releaseDate.Length >= 4) int.TryParse(releaseDate[..4], out year);

                    // Fetch details for runtime and director
                    int duration = 0;
                    string director = "";
                    try
                    {
                        var detailUrl = $"https://api.themoviedb.org/3/movie/{tmdbId}?api_key={apiKey}&append_to_response=credits";
                        var detailResponse = await _http.GetStringAsync(detailUrl);
                        var detailJson = JsonDocument.Parse(detailResponse);

                        if (detailJson.RootElement.TryGetProperty("runtime", out var rt) && rt.ValueKind == JsonValueKind.Number)
                            duration = rt.GetInt32();

                        if (detailJson.RootElement.TryGetProperty("credits", out var credits) &&
                            credits.TryGetProperty("crew", out var crew))
                        {
                            foreach (var member in crew.EnumerateArray())
                            {
                                if (member.TryGetProperty("job", out var job) && job.GetString() == "Director")
                                {
                                    director = member.GetProperty("name").GetString() ?? "";
                                    break;
                                }
                            }
                        }
                    }
                    catch { /* skip details on error */ }

                    // Small delay to respect TMDB rate limits
                    await Task.Delay(50);

                    var film = new Film
                    {
                        Title = title,
                        Description = overview.Length > 1000 ? overview[..1000] : overview,
                        Year = year,
                        Duration = duration > 0 ? duration : 120,
                        Director = director.Length > 100 ? director[..100] : director,
                        PosterUrl = posterPath != null ? $"https://image.tmdb.org/t/p/w500{posterPath}" : "",
                        CreatedAt = DateTime.UtcNow,
                        Genre = new List<Genre> { genres[genreName] }
                    };

                    _context.Films.Add(film);
                    filmsImported++;
                }

                await _context.SaveChangesAsync();
            }

            // 3. Import TV series per genre
            foreach (var (genreName, tmdbGenreId) in TvGenreMap)
            {
                var url = $"https://api.themoviedb.org/3/discover/tv?api_key={apiKey}&with_genres={tmdbGenreId}&sort_by=popularity.desc&page=1";
                var response = await _http.GetStringAsync(url);
                var json = JsonDocument.Parse(response);
                var results = json.RootElement.GetProperty("results");

                foreach (var show in results.EnumerateArray().Take(20))
                {
                    var title = show.GetProperty("name").GetString();
                    if (string.IsNullOrEmpty(title)) continue;

                    // Skip if already exists
                    var exists = await _context.Series.AnyAsync(s => s.Title == title);
                    if (exists) continue;

                    var tmdbId = show.GetProperty("id").GetInt32();
                    var posterPath = show.TryGetProperty("poster_path", out var pp) && pp.ValueKind != JsonValueKind.Null
                        ? pp.GetString() : null;
                    var overview = show.TryGetProperty("overview", out var ov) ? ov.GetString() ?? "" : "";
                    var firstAirDate = show.TryGetProperty("first_air_date", out var fad) ? fad.GetString() ?? "" : "";
                    int year = 0;
                    if (firstAirDate.Length >= 4) int.TryParse(firstAirDate[..4], out year);

                    // Fetch details for seasons and creator
                    int seasons = 1;
                    string creator = "";
                    try
                    {
                        var detailUrl = $"https://api.themoviedb.org/3/tv/{tmdbId}?api_key={apiKey}";
                        var detailResponse = await _http.GetStringAsync(detailUrl);
                        var detailJson = JsonDocument.Parse(detailResponse);

                        if (detailJson.RootElement.TryGetProperty("number_of_seasons", out var ns) && ns.ValueKind == JsonValueKind.Number)
                            seasons = ns.GetInt32();

                        if (detailJson.RootElement.TryGetProperty("created_by", out var cb))
                        {
                            foreach (var person in cb.EnumerateArray())
                            {
                                creator = person.GetProperty("name").GetString() ?? "";
                                break;
                            }
                        }
                    }
                    catch { /* skip details on error */ }

                    await Task.Delay(50);

                    var series = new Series
                    {
                        Title = title,
                        Description = overview.Length > 1000 ? overview[..1000] : overview,
                        Year = year,
                        Seasons = seasons,
                        Director = creator.Length > 100 ? creator[..100] : creator,
                        PosterUrl = posterPath != null ? $"https://image.tmdb.org/t/p/w500{posterPath}" : "",
                        CreatedAt = DateTime.UtcNow,
                        Genre = new List<Genre> { genres[genreName] }
                    };

                    _context.Series.Add(series);
                    seriesImported++;
                }

                await _context.SaveChangesAsync();
            }

            return Ok($"Import complete. {filmsImported} films and {seriesImported} series imported.");
        }

        [HttpPost("seed-interactions")]
        public async Task<IActionResult> SeedInteractions()
        {
            var random = new Random(42);

            var users = await _context.Users.ToListAsync();
            if (!users.Any()) return BadRequest("No users found.");

            var films = await _context.Films.Take(30).ToListAsync();
            var series = await _context.Series.Take(30).ToListAsync();

            if (!films.Any() && !series.Any())
                return BadRequest("No films or series found.");

            var sampleComments = new[]
            {
                "Absolutely loved this one!", "A masterpiece of its genre.",
                "Great watch, highly recommended.", "Really enjoyed the storyline.",
                "Fantastic performances all around.", "One of the best I've seen.",
                "Kept me hooked from start to finish.", "A bit slow but worth it.",
                "Superb cinematography and writing.", "Could not stop watching!",
                "Interesting concept, well executed.", "A solid entry in the genre."
            };

            int ratingsAdded = 0;
            int commentsAdded = 0;

            foreach (var user in users)
            {
                // Pick ~10 random films to rate
                var filmsToRate = films.OrderBy(_ => random.Next()).Take(Math.Min(10, films.Count)).ToList();
                foreach (var film in filmsToRate)
                {
                    var exists = await _context.Ratings.AnyAsync(r => r.UserId == user.Id && r.FilmId == film.Id);
                    if (exists) continue;

                    _context.Ratings.Add(new Rating
                    {
                        UserId = user.Id,
                        FilmId = film.Id,
                        Value = random.Next(6, 11),
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                    });
                    ratingsAdded++;
                }

                // Pick ~10 random series to rate
                var seriesToRate = series.OrderBy(_ => random.Next()).Take(Math.Min(10, series.Count)).ToList();
                foreach (var s in seriesToRate)
                {
                    var exists = await _context.Ratings.AnyAsync(r => r.UserId == user.Id && r.SeriesId == s.Id);
                    if (exists) continue;

                    _context.Ratings.Add(new Rating
                    {
                        UserId = user.Id,
                        SeriesId = s.Id,
                        Value = random.Next(6, 11),
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                    });
                    ratingsAdded++;
                }

                // Pick 2 random films to comment on
                var filmsToComment = films.OrderBy(_ => random.Next()).Take(2).ToList();
                foreach (var film in filmsToComment)
                {
                    _context.Comments.Add(new Comment
                    {
                        UserId = user.Id,
                        FilmId = film.Id,
                        Text = sampleComments[random.Next(sampleComments.Length)],
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                    });
                    commentsAdded++;
                }

                // Pick 2 random series to comment on
                var seriesToComment = series.OrderBy(_ => random.Next()).Take(2).ToList();
                foreach (var s in seriesToComment)
                {
                    _context.Comments.Add(new Comment
                    {
                        UserId = user.Id,
                        SeriesId = s.Id,
                        Text = sampleComments[random.Next(sampleComments.Length)],
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                    });
                    commentsAdded++;
                }

                await _context.SaveChangesAsync();
            }

            return Ok($"Seeded {ratingsAdded} ratings and {commentsAdded} comments across {users.Count} users.");
        }
    }
}