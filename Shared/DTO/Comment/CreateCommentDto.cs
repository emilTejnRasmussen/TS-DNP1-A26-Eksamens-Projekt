namespace DTO.Comment;

public record CreateCommentDto(
    string Body,
    int UserId,
    int? ParentCommentId
);