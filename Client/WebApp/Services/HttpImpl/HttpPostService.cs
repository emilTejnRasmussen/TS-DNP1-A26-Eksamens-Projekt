using DTO.Comment;
using DTO.Posts;
using DTO.PostVote;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpPostService(HttpClient client) : IPostService
{
    private readonly HttpClient _client = client;
    
    public Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        throw new NotImplementedException();
    }

    public Task<PostDetailsDto> GetPostAsync(int id, bool includeComments = false)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostDto>> GetPostsAsync(string? titleContains = null, int? userId = null, int? subforumId = null)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDto> AddCommentAsync(int postId, CreateCommentDto request)
    {
        throw new NotImplementedException();
    }

    public Task<List<CommentDto>> GetCommentsAsync(int postId)
    {
        throw new NotImplementedException();
    }

    public Task AddVoteAsync(int postId, CreatePostVoteDto request)
    {
        throw new NotImplementedException();
    }

    public Task<PostVoteSummaryDto> GetVotesAsync(int postId)
    {
        throw new NotImplementedException();
    }
}