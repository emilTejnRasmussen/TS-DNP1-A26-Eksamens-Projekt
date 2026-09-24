using DTO.Posts;
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

    [HttpGet]
    public IActionResult GetPosts(
        [FromQuery] string? titleContains = null,
        [FromQuery] int? userId = null
    )
    {
        var posts = postRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            posts = posts.Where(p => p.Title.Contains(titleContains));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        var dtos = posts
            .AsEnumerable()
            .Select(PostMapper.ToDto)
            .ToList();

        return Ok(dtos);
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        await postRepo.DeleteAsync(id);

        return NoContent();
    }
    
    
    //     POST    /posts/{postId}/comments
    
    
    //     GET     /posts/{postId}/comments
    //
    //     POST    /posts/{postId}/votes
    //     GET     /posts/{postId}/votes
}