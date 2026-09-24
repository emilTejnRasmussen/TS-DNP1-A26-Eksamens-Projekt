using Entities;

namespace DTO.PostVote;

public record PostVoteDto(
    int UserId,
    VoteType VoteType
);