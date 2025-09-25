using movie_service_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using movie_service_backend.DTO.UserDTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var token = await _userService.LoginAsync(dto);
        if (token == null)
        {
            return Unauthorized("Invalid username or password");
        }

        return Ok(new { Token = token });
    }
}
