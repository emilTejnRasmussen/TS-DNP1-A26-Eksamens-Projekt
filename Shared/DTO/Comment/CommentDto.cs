namespace DTO.Comment;

public record CommentDto(
    int Id,
    string Body,
    int UserId,
    string Username,
    int PostId,
    int? ParentCommentId
);