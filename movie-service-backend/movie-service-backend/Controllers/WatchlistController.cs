using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_service_backend.DTO.WatchlistDTOs;
using movie_service_backend.Interfaces;

namespace movie_service_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            _watchlistService = watchlistService;
        }

        // GET api/Watchlist/GetByUserId/5
        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var items = await _watchlistService.GetByUserIdAsync(userId);
            return Ok(items);
        }

        // POST api/Watchlist/Add
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] WatchlistAddDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _watchlistService.AddAsync(dto);
                return Ok(item);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE api/Watchlist/Remove/5
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var success = await _watchlistService.RemoveAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Removed from watchlist." });
        }

        // PUT api/Watchlist/MarkWatched/5
        [HttpPut("MarkWatched/{id}")]
        public async Task<IActionResult> MarkWatched(int id, [FromBody] WatchlistMarkWatchedDTO dto)
        {
            var item = await _watchlistService.MarkWatchedAsync(id, dto.Watched);
            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
