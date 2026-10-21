using DTO.Comment;
using Entities;

namespace Mapper;

public static class CommentMapper
{
    public static CommentDto ToDto(Comment comment)
    {
        return new CommentDto(
            comment.Id,
            comment.Body,
            comment.UserId,
            comment.PostId,
            comment.ParentCommentId
        );
    }

    public static Comment ToEntity(CreateCommentDto dto, int postId)
    {
        return new Comment(
            dto.Body,
            dto.UserId,
            postId,
            dto.ParentCommentId
        );
    }
}