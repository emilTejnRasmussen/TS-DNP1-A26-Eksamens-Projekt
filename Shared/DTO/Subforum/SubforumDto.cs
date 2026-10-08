namespace DTO.Subforum;

public record SubforumDto(
    int Id,
    string Name,
    string Description,
    int CreatorId,
    int PostCount
);