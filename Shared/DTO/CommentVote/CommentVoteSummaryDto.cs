using Entities;

namespace DTO.CommentVote;

public record CommentVoteSummaryDto(
    int Likes,
    int Dislikes,
    VoteType? UserVote
);