
using DTO.Subforum;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpSubforumService(HttpClient client) : ISubforumService
{
    public async Task<SubforumDto> AddSubforumAsync(
        CreateSubforumDto request)
    {
        var response = await client.PostAsJsonAsync("subforums", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content
            .ReadFromJsonAsync<SubforumDto>())!;
    }

    public async Task<SubforumDto> GetSubforumAsync(int id)
    {
        var response = await client.GetAsync($"subforums/{id}");

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content
            .ReadFromJsonAsync<SubforumDto>())!;
    }

    public async Task<List<SubforumDto>> GetSubforumsAsync(
        string? nameContains = null,
        int? creatorId = null)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            query.Add(
                $"nameContains={Uri.EscapeDataString(nameContains)}");
        }

        if (creatorId.HasValue)
        {
            query.Add($"creatorId={creatorId.Value}");
        }

        var url = "subforums";

        if (query.Count > 0)
        {
            url += "?" + string.Join("&", query);
        }

        var response = await client.GetAsync(url);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content
            .ReadFromJsonAsync<List<SubforumDto>>())!;
    }

    public async Task UpdateSubforumAsync(
        int id,
        UpdateSubforumDto request)
    {
        var response = await client.PatchAsJsonAsync(
            $"subforums/{id}",
            request
        );

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }

    public async Task DeleteSubforumAsync(int id)
    {
        var response = await client.DeleteAsync($"subforums/{id}");

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }
}
