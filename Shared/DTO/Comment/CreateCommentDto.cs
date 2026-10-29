namespace DTO.Comment;

public record CreateCommentDto(
    string Body,
    int? ParentCommentId
);