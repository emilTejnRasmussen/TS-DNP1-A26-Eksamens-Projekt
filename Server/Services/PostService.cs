using DTO.Comment;
using DTO.Posts;
using Mapper;
using RepositoryContract;
using Services.Interfaces;

namespace Services;

public class PostService(
    IPostRepository postRepo,
    ICommentRepository commentRepo,
    IUserRepository userRepo,
    ISubforumRepository subforumRepo) : IPostService
{
    public async Task<PostDetailsDto> GetPostByIdAsync(int id, bool includeComments = false)
    {
        var post = await postRepo.GetSingleAsync(id);
        var user = await userRepo.GetSingleAsync(post.UserId);
        var subforum = await subforumRepo.GetSingleAsync(post.SubforumId);

        List<CommentDto>? comments = null;

        if (includeComments)
        {
            comments = commentRepo.GetMany()
                .Where(c => c.PostId == id)
                .AsEnumerable()
                .Select(CommentMapper.ToDto)
                .ToList();
        }

        return PostMapper.ToDetailsDto(
            post,
            user.Username,
            subforum.Name,
            comments
        );
    }

    public async Task<IEnumerable<PostDto>> GetPostsAsync(
        string? titleContains = null,
        int? userId = null,
        int? subforumId = null)
    {
        var posts = postRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(titleContains))
        {
            posts = posts.Where(p =>
                p.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        if (subforumId.HasValue)
        {
            posts = posts.Where(p => p.SubforumId == subforumId.Value);
        }

        var dtos = new List<PostDto>();

        var comments = commentRepo.GetMany().ToList();

        foreach (var post in posts)
        {
            var user = await userRepo.GetSingleAsync(post.UserId);
            var subforum = await subforumRepo.GetSingleAsync(post.SubforumId);

            var commentCount = comments.Count(c => c.PostId == post.Id);

            dtos.Add(PostMapper.ToDto(
                post,
                user.Username,
                commentCount,
                subforum.Name
            ));
        }

        return dtos;
    }
}