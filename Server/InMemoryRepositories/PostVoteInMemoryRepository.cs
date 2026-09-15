using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class PostVoteInMemoryRepository : GenericInMemoryRepository<PostVote>, IPostVoteRepository
{
    public Task<int> CountByPostIdAndVoteTypeAsync(int postId, VoteType voteType)
    {
        var result = _entities.Count(vote => vote.PostId == postId && vote.VoteType == voteType);
        return Task.FromResult(result);
    }

    public Task<PostVote?> GetFromUserIdAndPostIdAsync(int userId, int postId)
    {
        var result = _entities.FirstOrDefault(pv => pv.UserId == userId && pv.PostId == postId);
        return Task.FromResult(result);
    }
}