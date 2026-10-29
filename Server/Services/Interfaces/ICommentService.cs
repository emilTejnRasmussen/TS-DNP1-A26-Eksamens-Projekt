using DTO.Comment;

namespace Services.Interfaces;

public interface ICommentService
{
    Task<CommentDto> CreateCommentAsync(int getUserId, int postId, CreateCommentDto request);
    Task<List<CommentDto>> GetCommentsForPostAsync(int postId);
}