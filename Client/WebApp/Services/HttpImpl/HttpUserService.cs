using DTO.User;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpUserService(HttpClient client) : IUserService
{
    private readonly HttpClient _client = client;

    public Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserDto>> GetUsersAsync(string? nameContains = null)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateUserAsync(int id, UpdateUserDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }
}