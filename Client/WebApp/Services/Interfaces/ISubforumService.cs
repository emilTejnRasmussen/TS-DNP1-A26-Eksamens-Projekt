using DTO.Subforum;

namespace WebApp.Services.Interfaces;

public interface ISubforumService
{
    Task<SubforumDto> AddSubforumAsync(CreateSubforumDto request);
    Task<SubforumDto> GetSubforumAsync(int id);
    Task<List<SubforumDto>> GetSubforumsAsync(string? nameContains = null);
    Task UpdateSubforumAsync(int id, UpdateSubforumDto request);
    Task DeleteSubforumAsync(int id);
}