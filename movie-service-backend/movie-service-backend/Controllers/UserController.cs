using movie_service_backend.DTO.UserDTOs;
using movie_service_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace movie_service_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service) {
            _userService = service;
        }
        [HttpPost("CreateUser")]
        public async Task<ActionResult> Create([FromBody] UserCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.CreateUserAsync(dto);
            return Ok("User successfully created.");
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }
        [Authorize]
        [HttpGet("GetByID{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [Authorize]
        [HttpPut("UpdateUser{id}")]
        public async Task<IActionResult> Update(int id, UserCreateDTO dto)
        {
            await _userService.UpdateUserAsync(id, dto);
            return Ok("User successfully updated.");
        }
        [Authorize]
        [HttpDelete("DeleteUser{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok("User successfully deleted.");
        }
    }   

}
