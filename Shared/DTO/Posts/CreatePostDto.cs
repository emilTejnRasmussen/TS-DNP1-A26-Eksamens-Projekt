namespace DTO.Posts;

public record CreatePostDto(
    string Title,
    string Body,
    int SubforumId
);