using Entities;

namespace RepositoryContract;

public interface IPostVoteRepository : IRepository<PostVote>
{
    Task<int> CountByPostIdAndVoteTypeAsync(int postId, VoteType voteType);
    Task<PostVote?> GetFromUserIdAndPostIdAsync(int userId, int postId);
}