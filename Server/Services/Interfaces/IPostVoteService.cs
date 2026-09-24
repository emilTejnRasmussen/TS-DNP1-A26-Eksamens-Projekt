using DTO.PostVote;
using Entities;

namespace Services;

public interface IPostVoteService
{
    Task SetVoteAsync(int postId, int userId, VoteType voteType);
    
    Task<PostVoteSummaryDto> GetVotesAsync(int postId);
}