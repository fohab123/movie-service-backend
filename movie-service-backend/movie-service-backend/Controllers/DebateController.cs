using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_service_backend.DTO.DebatePostDTOs;
using movie_service_backend.Interfaces;

namespace movie_service_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebateController : ControllerBase
    {
        private readonly IDebateService _service;

        public DebateController(IDebateService service)
        {
            _service = service;
        }

        // GET /api/debate?sort=newest&filmId=1&userId=5
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? sort,
            [FromQuery] int? filmId,
            [FromQuery] int? seriesId,
            [FromQuery] int? userId)
        {
            var result = await _service.GetAllAsync(sort, filmId, seriesId, userId);
            return Ok(result);
        }

        // GET /api/debate/{id}?userId=5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetThread(int id, [FromQuery] int? userId)
        {
            var result = await _service.GetDebateThreadAsync(id, userId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // GET /api/debate/film/{filmId}?userId=5
        [HttpGet("film/{filmId}")]
        public async Task<IActionResult> GetByFilm(int filmId, [FromQuery] int? userId)
        {
            var result = await _service.GetByFilmAsync(filmId, userId);
            return Ok(result);
        }

        // GET /api/debate/series/{seriesId}?userId=5
        [HttpGet("series/{seriesId}")]
        public async Task<IActionResult> GetBySeries(int seriesId, [FromQuery] int? userId)
        {
            var result = await _service.GetBySeriesAsync(seriesId, userId);
            return Ok(result);
        }

        // POST /api/debate/Create?userId=5
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePost([FromBody] DebatePostCreateDTO dto, [FromQuery] int userId)
        {
            var result = await _service.CreatePostAsync(dto, userId);
            return Ok(result);
        }

        // DELETE /api/debate/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var deleted = await _service.DeletePostAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // POST /api/debate/like/{postId}?userId=5
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> ToggleLike(int postId, [FromQuery] int userId)
        {
            try
            {
                await _service.ToggleLikeAsync(userId, postId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT /api/debate/view/{id}
        [HttpPut("view/{id}")]
        public async Task<IActionResult> IncrementView(int id)
        {
            await _service.IncrementViewAsync(id);
            return NoContent();
        }
    }
}
