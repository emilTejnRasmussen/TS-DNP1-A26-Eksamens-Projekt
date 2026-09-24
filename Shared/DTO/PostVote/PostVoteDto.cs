using Entities;

namespace DTO.PostVote;

public record PostVoteDto(
    int id,
    int UserId,
    VoteType VoteType
);