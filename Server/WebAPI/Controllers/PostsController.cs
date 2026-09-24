using DTO.Posts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;
using WebApi.Mappers;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(IPostRepository postRepo) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPost([FromBody] CreatePostDto request)
    {
        var created = await postRepo.AddAsync(PostMapper.ToEntity(request));

        return Created(
            $"/posts/{created.Id}",
            PostMapper.ToDto(created)
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPost([FromRoute] int id)
    {
        var post = await postRepo.GetSingleAsync(id);

        return Ok(PostMapper.ToDto(post));
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
    {
        var postToUpdate = await postRepo.GetSingleAsync(id);

        postToUpdate.Title = request.Title;
        postToUpdate.Body = request.Body;

        await postRepo.UpdateAsync(postToUpdate);

        return NoContent();
    }
}