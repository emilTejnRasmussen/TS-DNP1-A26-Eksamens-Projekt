using Entities;

namespace DTO.CommentVote;

public record CreateCommentVoteDto(
    int UserId,
    VoteType VoteType
);