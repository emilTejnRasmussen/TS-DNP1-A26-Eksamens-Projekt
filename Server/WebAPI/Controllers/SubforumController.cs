using DTO.Subforum;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;
using WebApi.Mappers;

namespace WebApi.Controllers;

[ApiController]
[Route("subforums")]
public class SubforumController(
    ISubforumRepository subforumRepo,
    IPostRepository postRepo
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddSubforum([FromBody] CreateSubforumDto request)
    {
        var createdSubforum = await subforumRepo.AddAsync(SubforumMapper.ToEntity(request));

        return Created($"/subforums/{createdSubforum.Id}", SubforumMapper.ToDto(createdSubforum, 0));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSubforums([FromQuery] string? nameContains = null)
    {
        var subforums = subforumRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            subforums = subforums.Where(s => s.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }

        var dtos = new List<SubforumDto>();

        foreach (var subforum in subforums)
        {
            var postCount = await postRepo.CountBySubforumIdAsync(subforum.Id);
            dtos.Add(SubforumMapper.ToDto(subforum, postCount));
        }

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSubforumById([FromRoute] int id)
    {
        var subforum = await subforumRepo.GetSingleAsync(id);
        var postCount = await postRepo.CountBySubforumIdAsync(id);

        return Ok(SubforumMapper.ToDto(subforum, postCount));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateSubforum(
        [FromRoute] int id,
        [FromBody] UpdateSubforumDto request)
    {
        var subforumToUpdate = await subforumRepo.GetSingleAsync(id);

        subforumToUpdate.Name = request.Name;
        subforumToUpdate.Description = request.Description;

        await subforumRepo.UpdateAsync(subforumToUpdate);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSubforum([FromRoute] int id)
    {
        await subforumRepo.DeleteAsync(id);

        return NoContent();
    }
}