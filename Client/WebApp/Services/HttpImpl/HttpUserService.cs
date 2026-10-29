using DTO.User;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpUserService(HttpClient client) : IUserService
{
    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        var response = await client.PostAsJsonAsync("users", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        var response = await client.GetAsync($"users/{id}");

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
    }

    public async Task<List<UserDto>> GetUsersAsync(string? nameContains = null)
    {
        var url = "users";

        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            url += $"?nameContains={Uri.EscapeDataString(nameContains)}";
        }

        var response = await client.GetAsync(url);

        await HttpResponseHelper.EnsureSuccessAsync(response);

        return (await response.Content.ReadFromJsonAsync<List<UserDto>>())!;
    }

    public async Task UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var response = await client.PatchAsJsonAsync($"users/{id}", request);

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await client.DeleteAsync($"users/{id}");

        await HttpResponseHelper.EnsureSuccessAsync(response);
    }
}