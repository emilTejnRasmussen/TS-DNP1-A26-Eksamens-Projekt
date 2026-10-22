using DTO.Subforum;
using WebApp.Services.Interfaces;

namespace WebApp.Services.HttpImpl;

public class HttpSubforumService(HttpClient client) : ISubforumService
{
    private readonly HttpClient _client = client;
    
    public Task<SubforumDto> AddSubforumAsync(CreateSubforumDto request)
    {
        throw new NotImplementedException();
    }

    public Task<SubforumDto> GetSubforumAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<SubforumDto>> GetSubforumsAsync(string? nameContains = null)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSubforumAsync(int id, UpdateSubforumDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSubforumAsync(int id)
    {
        throw new NotImplementedException();
    }
}