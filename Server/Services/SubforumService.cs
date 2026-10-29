using DTO.Subforum;
using Entities;
using Mapper;
using RepositoryContract;
using Services.Exceptions;
using Services.Interfaces;

namespace Services;

public class SubforumService(
    ISubforumRepository subforumRepo,
    IPostRepository postRepo) : ISubforumService
{
    public async Task<SubforumDto> CreateSubforumAsync(int userId, CreateSubforumDto request)
    {
        var createdSubforum = await subforumRepo.AddAsync(SubforumMapper.ToEntity(request, userId));

        return SubforumMapper.ToDto(createdSubforum, 0);
    }

    public async Task<List<SubforumDto>> GetManyAsync(string? nameContains = null, int? creatorId = null)
    {
        var subforums = subforumRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(nameContains))
        {
            subforums = subforums.Where(s => s.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }

        if (creatorId.HasValue)
        {
            subforums = subforums.Where(s => s.CreatorId == creatorId);
        }

        var dtos = new List<SubforumDto>();

        foreach (var subforum in subforums)
        {
            var postCount = await postRepo.CountBySubforumIdAsync(subforum.Id);
            dtos.Add(SubforumMapper.ToDto(subforum, postCount));
        }

        return dtos;
    }

    public async Task<SubforumDto> GetSingleAsync(int subforumId)
    {
        var subforum = await subforumRepo.GetSingleAsync(subforumId);
        var postCount = await postRepo.CountBySubforumIdAsync(subforumId);

        return SubforumMapper.ToDto(subforum, postCount);
    }

    public async Task UpdateAsync(int userId, int subforumId, UpdateSubforumDto request)
    {
        var subforumToUpdate = await subforumRepo.GetSingleAsync(subforumId);

        EnsureUserIsSubforumCreator(userId, subforumToUpdate);
            
        subforumToUpdate.Name = request.Name;
        subforumToUpdate.Description = request.Description;
        subforumToUpdate.Color = request.Color;
        subforumToUpdate.Symbol = request.Symbol;

        await subforumRepo.UpdateAsync(subforumToUpdate);
    }

    public async Task DeleteAsync(int userId, int subforumId)
    {
        var subforumToDelete = await subforumRepo.GetSingleAsync(subforumId);
        
        EnsureUserIsSubforumCreator(userId, subforumToDelete);
        
        await subforumRepo.DeleteAsync(subforumId);
    }

    private static void EnsureUserIsSubforumCreator(int userId, Subforum subforum)
    {
        if (userId != subforum.CreatorId)
        {
            throw new ForbiddenOperationException("You are not allowed to modify this subforum.");
        }
    }
}