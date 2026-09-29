namespace DTO.Comment;

public record CreateReplyDto(
    int UserId,
    string Body
);