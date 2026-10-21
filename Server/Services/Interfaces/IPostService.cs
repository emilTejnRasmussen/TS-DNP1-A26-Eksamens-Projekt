using DTO.Posts;

namespace Services.Interfaces;

public interface IPostService
{
    Task<PostDetailsDto> GetPostByIdAsync(
        int id,
        bool includeComments = false
    );

    Task<IEnumerable<PostDto>> GetPostsAsync(
        string? titleContains = null,
        int? userId = null,
        int? subforumId = null
    );
}