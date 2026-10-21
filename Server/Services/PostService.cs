using DTO.Comment;
using DTO.Posts;
using Mapper;
using RepositoryContract;
using Services.Interfaces;

namespace Services;

public class PostService(
    IPostRepository postRepo,
    ICommentRepository commentRepo,
    IUserRepository userRepo) : IPostService
{
    public async Task<PostDetailsDto> GetPostByIdAsync(int id, bool includeComments = false)
    {
        var post = await postRepo.GetSingleAsync(id);
        var user = await userRepo.GetSingleAsync(post.UserId);
        
        List<CommentDto>? comments = null;

        if (includeComments)
        {
            comments = commentRepo.GetMany()
                .Where(c => c.PostId == id)
                .AsEnumerable()
                .Select(CommentMapper.ToDto)
                .ToList();
        }

        return PostMapper.ToDetailsDto(post, user.Username, comments);
    }

    public Task<IEnumerable<PostDto>> GetPostsAsync(
        string? titleContains = null,
        int? userId = null,
        int? subforumId = null)
    {
        var posts = postRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            posts = posts.Where(p => p.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        if (subforumId.HasValue)
        {
            posts = posts.Where(p => p.SubforumId == subforumId.Value);
        }

        IEnumerable<PostDto> dtos = posts
            .AsEnumerable()
            .Select(PostMapper.ToDto)
            .ToList();

        return Task.FromResult(dtos);
    }
}