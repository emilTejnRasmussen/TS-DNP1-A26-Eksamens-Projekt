using DTO.Subforum;

namespace Services.Interfaces;

public interface ISubforumService
{
    Task<SubforumDto> CreateSubforumAsync(int userId, CreateSubforumDto request);
    Task<List<SubforumDto>> GetManyAsync(string? nameContains = null, int? creatorId = null);
    Task<SubforumDto> GetSingleAsync(int subforumId);
    Task UpdateAsync(int userId, int subforumId, UpdateSubforumDto request);
    Task DeleteAsync(int userId, int subforumId);
}