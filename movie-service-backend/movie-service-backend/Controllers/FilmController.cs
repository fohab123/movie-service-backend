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

        [HttpPost("CreateFilm")]
        public async Task<IActionResult> Create([FromBody] FilmCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _filmService.CreateAsync(dto);
            return Ok("Film added successfully.");
        }

    }
}
