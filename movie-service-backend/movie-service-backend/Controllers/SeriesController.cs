using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_service_backend.DTO.SeriesDTOs;
using movie_service_backend.Interfaces;
using movie_service_backend.Services;

namespace movie_service_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController :ControllerBase
    {
        private readonly ISeriesService _seriesService;

        public SeriesController(ISeriesService seriesService)
        {

            _seriesService = seriesService;
        }
        [HttpPost("CreateFilm")]
        public async Task<IActionResult> Create([FromBody] SeriesCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _seriesService.CreateSeriesAsync(dto);
            return Ok("Series added successfully.");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _seriesService.GetAllSeriesAsync());
        }
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var series = await _seriesService.GetSeriesByIdAsync(id);
            if (series == null) return NotFound();
            return Ok(series);
        }
        [HttpPut("UpdateSeries/{id}")]
        public async Task<IActionResult> Update(int id, SeriesUpdateDTO dto)
        {
            await _seriesService.UpdateSeriesAsync(id,dto);
            return Ok("Series successfully updated");
        }
        [HttpDelete("DeleteSeries/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _seriesService.DeleteSeriesAsync(id);
            return Ok("Series deleted successfully");
        }
        [HttpGet("OrderedByTimeAdded")]
        public async Task<IActionResult> GetAllOrderByTimeAdded()
        {
            var film = await _seriesService.GetAllSortedByDateAsync();
            return Ok(film);
        }
    }
}
