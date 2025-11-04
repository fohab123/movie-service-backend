using movie_service_backend.DTO.FilmDTOs;
using movie_service_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace movie_service_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmController(IFilmService filmService)
        {
            _filmService = filmService;
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("CreateFilm")]
        public async Task<IActionResult> Create([FromBody] FilmCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _filmService.CreateFilmAsync(dto);
            return Ok("Film added successfully.");
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _filmService.GetAllFilmsAsync());
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var film = await _filmService.GetFilmByIdAsync(id);
            if (film == null) return NotFound();
            return Ok(film);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("UpdateFilm/{id}")]
        public async Task<IActionResult> Update(int id, FilmCreateDTO dto)
        {
            await _filmService.UpdateFilmAsync(id, dto);
            return Ok("Film successfully updated.");
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("DeleteFilm/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _filmService.DeleteFilmAsync(id);
            return Ok("Film successfully deleted.");
        }
        [HttpGet("GroupedByGenre")]
        public async Task<IActionResult> GetGroupedByGenre()
        {
            var result = await _filmService.GetFilmsGroupedByGenreAsync();
            return Ok(result);
        }
        [HttpGet("OrderedByTimeAdded")]
        public async Task<IActionResult> GetAllOrderByTimeAdded()
        {
            var film = await _filmService.GetAllSortedByDateAsync();
            return Ok(film);
        }
    }
}
