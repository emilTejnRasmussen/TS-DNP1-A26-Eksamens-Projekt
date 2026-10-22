using DTO.Posts;
using DTO.Comment;
using DTO.PostVote;

namespace WebApp.Services.Interfaces;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDetailsDto> GetPostAsync(int id, bool includeComments = false);
    Task<List<PostDto>> GetPostsAsync(
        string? titleContains = null,
        int? userId = null,
        int? subforumId = null);
    Task UpdatePostAsync(int id, UpdatePostDto request);
    Task DeletePostAsync(int id);

    Task<CommentDto> AddCommentAsync(int postId, CreateCommentDto request);
    Task<List<CommentDto>> GetCommentsAsync(int postId);

    Task AddVoteAsync(int postId, CreatePostVoteDto request);
    Task<PostVoteSummaryDto> GetVotesAsync(int postId);
}