using DTO.Comment;
using Entities;
using Mapper;
using RepositoryContract;
using Services.Exceptions;
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

    public async Task<CommentDto> ReplyAsync(int userId, int commentId, CreateReplyDto request)
    {
        var user = await userRepo.GetSingleAsync(userId);
        var parentComment = await commentRepo.GetSingleAsync(commentId);

        var reply = new Comment(
            request.Body,
            userId,
            parentComment.PostId,
            parentComment.Id
        );

        var createdReply = await commentRepo.AddAsync(reply);

        return CommentMapper.ToDto(createdReply, user.Username);
    }

    public async Task<CommentDto> GetSingleAsync(int commentId)
    {
        var comment = await commentRepo.GetSingleAsync(commentId);
        var user = await userRepo.GetSingleAsync(comment.UserId);

        return CommentMapper.ToDto(comment, user.Username);
    }

    public async Task<List<CommentDto>> GetManyAsync(int? userId, int? postId)
    {
        var comments = commentRepo.GetMany();
        
        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        var dtos = new List<CommentDto>();
        
        foreach (var comment in comments)
        {
            var user = await userRepo.GetSingleAsync(comment.UserId);
            dtos.Add(CommentMapper.ToDto(comment, user.Username));
        }

        return dtos;
    }

    public async Task UpdateCommentAsync(int userId, int commentId, UpdateCommentDto request)
    {
        var commentToUpdate = await commentRepo.GetSingleAsync(commentId);
        
        EnsureUserIsAuthorOfComment(userId, commentToUpdate);

        commentToUpdate.Body = request.Body;

        await commentRepo.UpdateAsync(commentToUpdate);
    }

    public async Task DeleteAsync(int userId, int commentId)
    {
        var commentToDelete = await commentRepo.GetSingleAsync(commentId);
        
        EnsureUserIsAuthorOfComment(userId, commentToDelete);

        await commentRepo.DeleteAsync(commentId);
    }

    private void EnsureUserIsAuthorOfComment(int userId, Comment comment)
    {
        if (userId != comment.UserId)
        {
            throw new ForbiddenOperationException("You are not allowed to modify this comment.");
        }
    }
}