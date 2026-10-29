using DTO.Comment;

namespace Services.Interfaces;

public interface ICommentService
{
    Task<CommentDto> CreateCommentAsync(int getUserId, int postId, CreateCommentDto request);
    Task<List<CommentDto>> GetCommentsForPostAsync(int postId);
    Task<CommentDto> ReplyAsync(int getUserId, int commentId, CreateReplyDto request);
    Task<CommentDto> GetSingleAsync(int commentId);
    Task<List<CommentDto>> GetManyAsync(int? userId, int? postId);
    Task UpdateCommentAsync(int userId, int commentId, UpdateCommentDto request);
    Task DeleteAsync(int userId, int commentId);
}