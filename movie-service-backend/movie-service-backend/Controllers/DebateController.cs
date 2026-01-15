using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_service_backend.DTO;
using movie_service_backend.DTO.DebatePostDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Services;
using System.Security.Claims;

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
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] DebatePostCreateDTO dto, int userId)
        {
            var result = await _service.CreatePostAsync(dto, userId);
            return Ok(result);
        }

        [HttpGet("roots")]
        public async Task<IActionResult> GetRootPosts()
        {
            var result = await _service.GetAllRootPostsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetThread(int id)
        {
            var result = await _service.GetDebateThreadAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var deleted = await _service.DeletePostAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
        [Authorize]
        [HttpPost("like/{postId}")]
        public async Task<IActionResult> ToggleLike(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                await _service.ToggleLikeAsync(userId, postId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
