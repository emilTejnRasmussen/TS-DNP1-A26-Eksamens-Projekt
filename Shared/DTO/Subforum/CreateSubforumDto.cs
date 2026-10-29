namespace DTO.Subforum;

public record CreateSubforumDto(
    string Name,
    string Description,
    int CreatorId,
    string Color,
    string Symbol
);