using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_service_backend.DTO.CommentDTOs;
using movie_service_backend.DTO.RatingDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Services;

namespace movie_service_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost("CreateCommentForFilm")]
        public async Task<IActionResult> CreateForFilm([FromBody] CommentCreateFilmDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _commentService.CreateForFilmAsync(dto);
            return Ok("Comment added successfully.");
        }
        [HttpPost("CreateCommentForSeries")]
        public async Task<IActionResult> CreateForSeries([FromBody] CommentCreateSeriesDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _commentService.CreateForSeriesAsync(dto);
            return Ok("Comment added successfully.");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _commentService.GetAllAsync());
        }
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }
        [HttpPut("UpdateComment/{id}")]
        public async Task<IActionResult> Update(int id, CommentUpdateDTO dto)
        {
            await _commentService.UpdateAsync(id, dto);
            return Ok("Comment successfully updated.");
        }
        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.DeleteAsync(id);
            return Ok("Comment successfully deleted.");
        }
    }
}
