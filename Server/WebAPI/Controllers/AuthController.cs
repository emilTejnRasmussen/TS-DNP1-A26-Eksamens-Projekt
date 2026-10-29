using DTO.Auth;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
    {
        var response = await authService.RegisterAsync(request);
        return StatusCode(StatusCodes.Status201Created, response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request);
        return Ok(response);
    }
}