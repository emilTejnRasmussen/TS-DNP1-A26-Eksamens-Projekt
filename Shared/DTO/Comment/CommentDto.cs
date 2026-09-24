namespace DTO.Comment;

public record CommentDto(
    int Id,
    string Body,
    int UserId,
    int PostId,
    int? ParentCommentId
);