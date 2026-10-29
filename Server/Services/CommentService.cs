using DTO.Comment;
using Entities;
using Mapper;
using RepositoryContract;
using Services.Interfaces;

namespace Services;

public class CommentService(
    ICommentRepository commentRepo,
    IPostRepository postRepo,
    IUserRepository userRepo) : ICommentService
{
    public async Task<CommentDto> CreateCommentAsync(int userId, int postId, CreateCommentDto request)
    {
        var user = await userRepo.GetSingleAsync(userId);
        await postRepo.GetSingleAsync(postId);

        var createdComment = await commentRepo.AddAsync(CommentMapper.ToEntity(request, postId, userId));

        return CommentMapper.ToDto(createdComment, user.Username);
    }

    public async Task<List<CommentDto>> GetCommentsForPostAsync(int postId)
    {
        await postRepo.GetSingleAsync(postId);
        
        var comments = commentRepo.GetMany()
            .Where(comment => comment.PostId == postId)
            .OrderByDescending(c => c.CreatedAt);

        var commentDtos = new List<CommentDto>();

        foreach (var comment in comments)
        {
            var user = await userRepo.GetSingleAsync(comment.UserId);
            commentDtos.Add(CommentMapper.ToDto(comment, user.Username));
        }

        return commentDtos;
    }
}