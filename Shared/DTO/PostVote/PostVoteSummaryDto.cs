using Entities;

namespace DTO.PostVote;

public record PostVoteSummaryDto(
    int Likes,
    int Dislikes
);