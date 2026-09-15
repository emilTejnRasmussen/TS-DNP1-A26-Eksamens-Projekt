using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class CommentVoteInMemoryRepository : GenericInMemoryRepository<CommentVote>, ICommentVoteRepository
{
    public CommentVoteInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter().GetResult();
    }

    public Task<CommentVote?> GetFromUserIdAndCommentIdAsync(int userId, int commentId)
    {
        var existingEntity = _entities.SingleOrDefault(cv => cv.UserId == userId && cv.CommentId == commentId);
        return Task.FromResult(existingEntity);
    }

    public Task<int> CountByCommentIdAndVoteTypeAsync(int commentId, VoteType voteType)
    {
        var result = _entities.Count(vote => vote.CommentId == commentId && vote.VoteType == voteType);
        return Task.FromResult(result);
    }

    private async Task SeedDataAsync()
    {
        await AddAsync(new CommentVote(
            1,
            1,
            VoteType.Like));

        await AddAsync(new CommentVote(
            3,
            1,
            VoteType.Like));

        await AddAsync(new CommentVote(
            2,
            5,
            VoteType.Dislike));
    }
}