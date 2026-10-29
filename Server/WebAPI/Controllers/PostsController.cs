using DTO.Comment;
using DTO.Posts;
using DTO.PostVote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebApi.Authentication;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(
    ICommentService commentService,
    IPostService postService,
    IPostVoteService postVoteService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPost(
        [FromRoute] int id,
        [FromQuery] bool includeComments = false)
    {
        var post = await postService.GetPostByIdAsync(id, includeComments);
        return Ok(post);
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts(
        [FromQuery] string? titleContains = null,
        [FromQuery] int? userId = null,
        [FromQuery] int? subforumId = null)
    {
        var posts = await postService.GetPostsAsync(
            titleContains,
            userId,
            subforumId
        );

        return Ok(posts);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddPost([FromBody] CreatePostDto request)
    {
        var response = await postService.CreatePostAsync(User.GetUserId(), request);

        return Created($"/posts/{response.Id}", response);
    }


    [Authorize]
    [HttpPatch("{postId:int}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int postId, [FromBody] UpdatePostDto request)
    {
        await postService.UpdatePostAsync(User.GetUserId(), postId, request);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{postId:int}")]
    public async Task<IActionResult> DeletePost([FromRoute] int postId)
    {
        await postService.DeleteAsync(User.GetUserId(), postId);

        return NoContent();
    }

    [Authorize]
    [HttpPost("{postId:int}/comments")]
    public async Task<IActionResult> AddComment(
        [FromRoute] int postId,
        [FromBody] CreateCommentDto request
    )
    {
        var response = await commentService.CreateCommentAsync(User.GetUserId(), postId, request);

        return Created($"/comments/{response.Id}", response);
    }

    [HttpGet("{postId:int}/comments")]
    public async Task<IActionResult> GetComments([FromRoute] int postId)
    {
        var response = await commentService.GetCommentsForPostAsync(postId);

        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("{postId:int}/votes")]
    public async Task<IActionResult> AddVote(
        [FromRoute] int postId,
        [FromBody] CreatePostVoteDto request
    )
    {
        await postVoteService.SetVoteAsync(
            postId,
            User.GetUserId(),
            request.VoteType
        );

        return NoContent();
    }

    [HttpGet("{postId:int}/votes")]
    public async Task<IActionResult> GetVotes([FromRoute] int postId)
    
    {
        int? userId = User.Identity?.IsAuthenticated == true
            ? User.GetUserId()
            : null;
        
        var votes = await postVoteService.GetVotesAsync(postId, userId);
        
        return Ok(votes);
    }
}