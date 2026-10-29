using DTO.Subforum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebApi.Authentication;

namespace WebApi.Controllers;

[ApiController]
[Route("subforums")]
public class SubforumController(ISubforumService subforumService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddSubforum([FromBody] CreateSubforumDto request)
    {
        var response = await subforumService.CreateSubforumAsync(User.GetUserId(), request);
        
        return Created($"/subforums/{response.Id}", response);
    }

    [HttpGet("{subforumId:int}")]
    public async Task<IActionResult> GetSubforumById([FromRoute] int subforumId)
    {
        var response = await subforumService.GetSingleAsync(subforumId);
        
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllSubforums(
        [FromQuery] string? nameContains = null,
        [FromQuery] int? creatorId = null)
    {
        var response = await subforumService.GetManyAsync(nameContains, creatorId);

        return Ok(response);
    }

    [Authorize]
    [HttpPatch("{subforumId:int}")]
    public async Task<IActionResult> UpdateSubforum(
        [FromRoute] int subforumId,
        [FromBody] UpdateSubforumDto request)
    {
        await subforumService.UpdateAsync(User.GetUserId(), subforumId, request);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{subforumId:int}")]
    public async Task<IActionResult> DeleteSubforum([FromRoute] int subforumId)
    {
        await subforumService.DeleteAsync(User.GetUserId(), subforumId);

        return NoContent();
    }
}