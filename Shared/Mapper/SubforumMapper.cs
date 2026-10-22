using DTO.Subforum;
using Entities;

namespace Mapper;

public static class SubforumMapper
{
    public static Subforum ToEntity(CreateSubforumDto dto)
    {
        var subforum = new Subforum(
            dto.Name,
            dto.Description,
            dto.CreatorId,
            dto.Color,
            dto.Symbol
        );

        return subforum;
    }

    public static SubforumDto ToDto(Subforum subforum, int postCount)
    {
        var dto = new SubforumDto(
            subforum.Id,
            subforum.Name,
            subforum.Description,
            subforum.CreatorId,
            postCount,
            subforum.Color,
            subforum.Symbol
        );

        return dto;
    }
}