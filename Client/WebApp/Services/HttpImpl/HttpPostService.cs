using DTO.Comment;
using DTO.Posts;
using DTO.PostVote;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpPostService(HttpClient client) : IPostService
{
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        var response = await client.PostAsJsonAsync("posts", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<PostDto>())!;
    }

    public async Task<PostDetailsDto> GetPostAsync(int id, bool includeComments = false)
    {
        var response = await client.GetAsync($"posts/{id}?includeComments={includeComments}");

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<PostDetailsDto>())!;
    }

    public async Task<List<PostDto>> GetPostsAsync(
        string? titleContains = null,
        int? userId = null,
        int? subforumId = null)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(titleContains))
            query.Add($"titleContains={Uri.EscapeDataString(titleContains)}");

        if (userId.HasValue)
            query.Add($"userId={userId.Value}");

        if (subforumId.HasValue)
            query.Add($"subforumId={subforumId.Value}");

        var url = "posts";

        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        var response = await client.GetAsync(url);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<List<PostDto>>())!;
    }

    public async Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        var response = await client.PatchAsJsonAsync($"posts/{id}", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }

    public async Task DeletePostAsync(int id)
    {
        var response = await client.DeleteAsync($"posts/{id}");

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }

    public async Task<CommentDto> AddCommentAsync(int postId, CreateCommentDto request)
    {
        var response = await client.PostAsJsonAsync($"posts/{postId}/comments", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<CommentDto>())!;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(int postId)
    {
        var response = await client.GetAsync($"posts/{postId}/comments");

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<List<CommentDto>>())!;
    }

    public async Task AddVoteAsync(int postId, CreatePostVoteDto request)
    {
        var response = await client.PostAsJsonAsync($"posts/{postId}/votes", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }

    public async Task<PostVoteSummaryDto> GetVotesAsync(int postId, int? userId = null)
    {
        var url = $"posts/{postId}/votes";

        if (userId.HasValue)
            url += $"?userId={userId.Value}";

        var response = await client.GetAsync(url);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<PostVoteSummaryDto>())!;
    }
}