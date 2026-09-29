using DTO.CommentVote;
using Entities;

namespace Services.Interfaces;

public interface ICommentVoteService
{
    Task<CommentVoteSummaryDto> GetVotesAsync(int commentId);
    Task SetVoteAsync(int commentId, int userId, VoteType voteType);
}