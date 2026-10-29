using DTO.User;
using Mapper;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserRepository userRepo) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var user = await userRepo.GetSingleAsync(id);
        return Ok(UserMapper.ToDto(user));
    }

    [HttpGet]
    public IActionResult GetUsers([FromQuery] string? nameContains = null)
    {
        var users = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            users = users.Where(u => u.Username.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }

        var dtos = users
            .AsEnumerable()
            .Select(UserMapper.ToDto)
            .ToList();

        return Ok(dtos);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] int id,
        [FromBody] UpdateUserDto request)
    {
        var userToUpdate = await userRepo.GetSingleAsync(id);

        userToUpdate.Username = request.Username;
        userToUpdate.hashedPassword = request.Password;

        await userRepo.UpdateAsync(userToUpdate);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await userRepo.DeleteAsync(id);

        return NoContent();
    }
}