namespace DTO.Posts;

public record PostDto(
    int Id,
    string Title,
    string Body,
    int UserId,
    string Username,
    int SubforumId,
    string SubforumName,
    int CommentCount,
    DateTimeOffset CreatedAt
);