using DTO.Comment;
using DTO.CommentVote;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpCommentService(HttpClient client) : ICommentService
{
    private readonly HttpClient _client = client;
    
    public Task<CommentDto> ReplyAsync(int commentId, CreateReplyDto request)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDto> GetCommentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<CommentDto>> GetCommentsAsync(int? userId = null, int? postId = null)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCommentAsync(int id, UpdateCommentDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCommentAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddVoteAsync(int commentId, CreateCommentVoteDto request)
    {
        throw new NotImplementedException();
    }

    public Task<CommentVoteSummaryDto> GetVotesAsync(int commentId)
    {
        throw new NotImplementedException();
    }
}