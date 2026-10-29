using DTO.Comment;
using DTO.CommentVote;
using Entities;
using Mapper;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;
using Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController(
    ICommentRepository commentRepo,
    ICommentVoteService commentVoteService,
    IUserRepository userRepo) : ControllerBase
{
    [HttpPost("{commentId:int}/replies")]
    public async Task<IActionResult> Reply([FromRoute] int commentId, [FromBody] CreateReplyDto request)
    {
        var user = await userRepo.GetSingleAsync(request.UserId);
        var parentComment = await commentRepo.GetSingleAsync(commentId);

        var reply = new Comment(
            request.Body,
            request.UserId,
            parentComment.PostId,
            parentComment.Id
        );

        var createdReply = await commentRepo.AddAsync(reply);

        return Created($"/comments/{createdReply.Id}", CommentMapper.ToDto(createdReply, user.Username));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetComment([FromRoute] int id)
    {
        var comment = await commentRepo.GetSingleAsync(id);
        var user = await userRepo.GetSingleAsync(comment.UserId);

        return Ok(CommentMapper.ToDto(comment, user.Username));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetComments(
        [FromQuery] int? userId = null,
        [FromQuery] int? postId = null)
    {
        var comments = commentRepo.GetMany();
        
        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        var dtos = new List<CommentDto>();
        
        foreach (var comment in comments)
        {
            var user = await userRepo.GetSingleAsync(comment.UserId);
            dtos.Add(CommentMapper.ToDto(comment, user.Username));
        }

        return Ok(dtos);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
    {
        var commentToUpdate = await commentRepo.GetSingleAsync(id);

        commentToUpdate.Body = request.Body;

        await commentRepo.UpdateAsync(commentToUpdate);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        await commentRepo.DeleteAsync(id);

        return NoContent();
    }

    [HttpPost("{commentId:int}/votes")]
    public async Task<IActionResult> Vote([FromRoute] int commentId, [FromBody] CreateCommentVoteDto request)
    {
        await commentVoteService.SetVoteAsync(
            commentId,
            request.UserId,
            request.VoteType
        );

        return NoContent();
    }
    
    [HttpGet("{commentId:int}/votes")]
    public async Task<IActionResult> GetVotes([FromRoute] int commentId)
    {
        var votes = await commentVoteService.GetVotesAsync(commentId);

        return Ok(votes);
    }
}