using DTO.Comment;
using DTO.Posts;
using DTO.PostVote;
using Mapper;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;
using Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(
    IPostRepository postRepo,
    ICommentRepository commentRepo,
    IUserRepository userRepo,
    ISubforumRepository subforumRepo,
    IPostService postService,
    IPostVoteService postVoteService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPost([FromBody] CreatePostDto request)
    {
        await userRepo.GetSingleAsync(request.UserId);
        await subforumRepo.GetSingleAsync(request.SubforumId);
        
        var created = await postRepo.AddAsync(PostMapper.ToEntity(request));

        return Created($"/posts/{created.Id}", PostMapper.ToDto(created));
    }

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
    public async Task<IActionResult> GetComments([FromRoute] int postId)
    {
        await postRepo.GetSingleAsync(postId);

        var commentDtos = commentRepo.GetMany()
            .Where(comment => comment.PostId == postId)
            .AsEnumerable()
            .Select(CommentMapper.ToDto)
            .ToList();

        return Ok(commentDtos);
    }


    [HttpPost("{postId:int}/votes")]
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
    public async Task<IActionResult> GetVotes([FromRoute] int postId)
    {
        var votes = await postVoteService.GetVotesAsync(postId);

        return Ok(votes);
    }
}