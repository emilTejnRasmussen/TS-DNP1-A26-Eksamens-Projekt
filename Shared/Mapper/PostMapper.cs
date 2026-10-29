using DTO.Posts;
using DTO.Comment;
using Entities;

namespace Mapper;

public static class PostMapper
{
    public static PostDto ToDto(Post post, string username, int commentCount, string subforumName)
    {
        return new PostDto(
            post.Id,
            post.Title,
            post.Body,
            post.UserId,
            username,
            post.SubforumId,
            subforumName,
            commentCount,
            post.CreatedAt
        );
    }

    public static PostDetailsDto ToDetailsDto(
        Post post,
        string username,
        string subforumName,
        List<CommentDto>? comments)
    {
        return new PostDetailsDto(
            post.Id,
            post.Title,
            post.Body,
            post.UserId,
            username,
            post.SubforumId,
            subforumName,
            comments
        );
    }

    public static Post ToEntity(CreatePostDto dto, int userId)
    {
        return new Post(
            dto.Title,
            dto.Body,
            userId,
            dto.SubforumId
        );
    }
}