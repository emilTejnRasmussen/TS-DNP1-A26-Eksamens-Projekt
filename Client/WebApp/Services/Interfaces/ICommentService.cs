using DTO.Comment;
using DTO.CommentVote;

namespace WebApp.Services.Interfaces;

public interface ICommentService
{
    Task<CommentDto> ReplyAsync(int commentId, CreateReplyDto request);
    Task<CommentDto> GetCommentAsync(int id);
    Task<List<CommentDto>> GetCommentsAsync(
        int? userId = null,
        int? postId = null);
    Task UpdateCommentAsync(int id, UpdateCommentDto request);
    Task DeleteCommentAsync(int id);

    Task AddVoteAsync(int commentId, CreateCommentVoteDto request);
    Task<CommentVoteSummaryDto> GetVotesAsync(int commentId, int? userId = null);
}