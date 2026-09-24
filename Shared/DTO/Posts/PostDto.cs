namespace DTO.Posts;

public record PostDto(
    int Id,
    string Title,
    string Body,
    int UserId,
    int SubforumId
);