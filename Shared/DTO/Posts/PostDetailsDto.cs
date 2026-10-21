using DTO.Comment;

namespace DTO.Posts;

public record PostDetailsDto(
    int Id,
    string Title,
    string Body,
    int UserId,
    string Username,
    int SubforumId,
    List<CommentDto>? Comments
);