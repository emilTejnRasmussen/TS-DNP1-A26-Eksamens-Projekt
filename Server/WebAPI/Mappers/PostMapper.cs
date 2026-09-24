using DTO.Posts;
using Entities;

namespace WebApi.Mappers;

public static class PostMapper
{
    public static PostDto ToDto(Post post)
    {
        return new PostDto(
            post.Id,
            post.Title,
            post.Body,
            post.UserId,
            post.SubforumId
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