using DTO.Posts;
using DTO.Comment;
using Entities;

namespace Mapper;

public static class PostMapper
{
    public static PostDto ToDto(Post post, string username, int commentCount)
    {
        return new PostDto(
            post.Id,
            post.Title,
            post.Body,
            post.UserId,
            username,
            post.SubforumId,
            commentCount
        );
    }

    public static PostDetailsDto ToDetailsDto(
        Post post,
        string username,
        List<CommentDto>? comments)
    {
        return new PostDetailsDto(
            post.Id,
            post.Title,
            post.Body,
            post.UserId,
            username,
            post.SubforumId,
            comments
        );
    }

    public static Post ToEntity(CreatePostDto dto)
    {
        return new Post(
            dto.Title,
            dto.Body,
            dto.UserId,
            dto.SubforumId
        );
    }
}