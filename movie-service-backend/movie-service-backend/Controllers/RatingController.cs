using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_service_backend.DTO.RatingDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Services;

namespace movie_service_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [HttpPost("CreateRatingForFilm")]
        public async Task<IActionResult> CreateForFilm([FromBody] RatingCreateFilmDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _ratingService.CreateForFilmAsync(dto);
            if (result == null)
                return BadRequest("You have already rated this film");
            return Ok("Rating added successfully.");
        }
        [HttpPost("CreateRatingForSeries")]
        public async Task<IActionResult> CreateForSeries([FromBody] RatingCreateSeriesDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _ratingService.CreateForSeriesAsync(dto);
            if (result == null)
                return BadRequest("You have already rated this series");
            return Ok("Rating added successfully.");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ratingService.GetAllAsync());
        }
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var rating = await _ratingService.GetByIdAsync(id);
            if (rating == null) return NotFound();
            return Ok(rating);
        }
        [HttpPut("UpdateRating/{id}")]
        public async Task<IActionResult> Update(int id, RatingUpdateDTO dto)
        {
            await _ratingService.UpdateAsync(id, dto);
            return Ok("Rating successfully updated.");
        }
        [HttpDelete("DeleteRating/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ratingService.DeleteAsync(id);
            return Ok("Rating successfully deleted.");
        }

        [HttpGet("film/{filmId}")]
        public async Task<IActionResult> GetByFilmId(int filmId)
        {
            return Ok(await _ratingService.GetByFilmIdAsync(filmId));
        }

        [HttpGet("series/{seriesId}")]
        public async Task<IActionResult> GetBySeriesId(int seriesId)
        {
            return Ok(await _ratingService.GetBySeriesIdAsync(seriesId));
        }

        [HttpGet("user/{userId}/film/{filmId}")]
        public async Task<IActionResult> GetUserFilmRating(int userId, int filmId)
        {
            var rating = await _ratingService.GetUserFilmRatingAsync(userId, filmId);
            if (rating == null) return NotFound();
            return Ok(rating);
        }

        [HttpGet("user/{userId}/series/{seriesId}")]
        public async Task<IActionResult> GetUserSeriesRating(int userId, int seriesId)
        {
            var rating = await _ratingService.GetUserSeriesRatingAsync(userId, seriesId);
            if (rating == null) return NotFound();
            return Ok(rating);
        }
    }

}
