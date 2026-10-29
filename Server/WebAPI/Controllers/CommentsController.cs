using DTO.Comment;
using DTO.CommentVote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebApi.Authentication;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController(
    ICommentService commentService,
    ICommentVoteService commentVoteService) : ControllerBase
{
    [Authorize]
    [HttpPost("{commentId:int}/replies")]
    public async Task<IActionResult> Reply([FromRoute] int commentId, [FromBody] CreateReplyDto request)
    {
        var response = await commentService.ReplyAsync(User.GetUserId(), commentId, request);

        return Created($"/comments/{response.Id}", response);
    }

    [HttpGet("{commentId:int}")]
    public async Task<IActionResult> GetComment([FromRoute] int commentId)
    {
        var response = await commentService.GetSingleAsync(commentId);

        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetComments(
        [FromQuery] int? userId = null,
        [FromQuery] int? postId = null)
    {
        var response = await commentService.GetManyAsync(userId, postId);
        
        return Ok(response);
    }

    [Authorize]
    [HttpPatch("{commentId:int}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int commentId, [FromBody] UpdateCommentDto request)
    {
        await commentService.UpdateCommentAsync(User.GetUserId(), commentId, request);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
    {
        await commentService.DeleteAsync(User.GetUserId(), commentId);

        return NoContent();
    }

    [Authorize]
    [HttpPost("{commentId:int}/votes")]
    public async Task<IActionResult> Vote([FromRoute] int commentId, [FromBody] CreateCommentVoteDto request)
    {
        await commentVoteService.SetVoteAsync(
            commentId,
            User.GetUserId(),
            request.VoteType
        );

        return NoContent();
    }
    
    [HttpGet("{commentId:int}/votes")]
    public async Task<IActionResult> GetVotes([FromRoute] int commentId)
    {
        int? userId = User.Identity?.IsAuthenticated == true
            ? User.GetUserId()
            : null;
        
        var votes = await commentVoteService.GetVotesAsync(commentId, userId);
        return Ok(votes);
    }
}