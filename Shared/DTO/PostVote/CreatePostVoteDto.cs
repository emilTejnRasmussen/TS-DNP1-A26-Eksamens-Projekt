using Entities;

namespace DTO.PostVote;

public record CreatePostVoteDto(
    int UserId,
    VoteType VoteType
);