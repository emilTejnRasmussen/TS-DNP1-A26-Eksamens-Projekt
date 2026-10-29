using DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebApi.Authentication;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var response = await userService.GetUserAsync(id);
        
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetUsers([FromQuery] string? nameContains = null)
    {
        var response = userService.GetMany(nameContains);
        
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("me")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        await userService.UpdateUserAsync(User.GetUserId(), request);

        return NoContent();
    }

    [Authorize]
    [HttpPut("me/password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await userService.ChangePasswordAsync(User.GetUserId(), request);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteUser()
    {
        await userService.DeleteAsync(User.GetUserId());

        return NoContent();
    }
}