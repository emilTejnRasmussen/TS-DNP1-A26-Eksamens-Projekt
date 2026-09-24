using DTO.Comment;
using DTO.Posts;
using DTO.PostVote;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;
using Services;
using WebApi.Mappers;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(
    IPostRepository postRepo,
    ICommentRepository commentRepo,
    IPostVoteService postVoteService) : ControllerBase
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


    [HttpPost("{postId:int}/comments")]
    public async Task<IActionResult> AddComment(
        [FromRoute] int postId,
        [FromBody] CreateCommentDto request
    )
    {
        await postRepo.GetSingleAsync(postId);

        var created = await commentRepo.AddAsync(CommentMapper.ToEntity(request, postId));

        return Created($"/comments/{created.Id}", CommentMapper.ToDto(created));
    }

    [HttpGet("{postId:int}/comments")]
    public IActionResult GetComments([FromRoute] int postId)
    {
        var commentDtos = commentRepo.GetMany()
            .Where(post => post.Id == postId)
            .AsEnumerable()
            .Select(CommentMapper.ToDto)
            .ToList();

        return Ok(commentDtos);
    }


    [HttpPut("{postId:int}/votes")]
    public async Task<IActionResult> AddVote(
        [FromRoute] int postId,
        [FromBody] CreatePostVoteDto request
    )
    {
        await postVoteService.SetVoteAsync(
            postId,
            request.UserId,
            request.VoteType
        );

        return NoContent();
    }

    [HttpGet("{postId:int}/votes")]
    public async Task<IActionResult> getVotes([FromRoute] int postId)
    {
        var votes = await postVoteService.GetVotesAsync(postId);

        var dtos = votes
            .Select(PostVoteMapper.ToDto)
            .ToList();

        return Ok(dtos);
    }
}