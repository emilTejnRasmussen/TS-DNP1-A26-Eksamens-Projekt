using DTO.Comment;
using DTO.CommentVote;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpCommentService(HttpClient client) : ICommentService
{
    public async Task<CommentDto> ReplyAsync(int commentId, CreateReplyDto request)
    {
        var response = await client.PostAsJsonAsync($"comments/{commentId}/replies", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<CommentDto>())!;
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        var response = await client.GetAsync($"comments/{id}");

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<CommentDto>())!;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(int? userId = null, int? postId = null)
    {
        var query = new List<string>();

        if (userId.HasValue)
            query.Add($"userId={userId.Value}");

        if (postId.HasValue)
            query.Add($"postId={postId.Value}");

        var url = "comments";

        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        var response = await client.GetAsync(url);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<List<CommentDto>>())!;
    }

    public async Task UpdateCommentAsync(int id, UpdateCommentDto request)
    {
        var response = await client.PatchAsJsonAsync($"comments/{id}", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }

    public async Task DeleteCommentAsync(int id)
    {
        var response = await client.DeleteAsync($"comments/{id}");

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }

    public async Task AddVoteAsync(int commentId, CreateCommentVoteDto request)
    {
        var response = await client.PostAsJsonAsync($"comments/{commentId}/votes", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }

    public async Task<CommentVoteSummaryDto> GetVotesAsync(int commentId, int? userId = null)
    {
        var url = $"comments/{commentId}/votes";

        if (userId.HasValue)
            url += $"?userId={userId.Value}";

        var response = await client.GetAsync(url);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<CommentVoteSummaryDto>())!;
    }
}