using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class CommentVoteInMemoryRepository : GenericInMemoryRepository<CommentVote>, ICommentVoteRepository
{
    public Task<CommentVote?> GetFromUserIdAndCommentIdAsync(int userId, int commentId)
    {
        var existingEntity = _entities.SingleOrDefault(cv => cv.UserId == userId && cv.CommentId == commentId);
        return Task.FromResult(existingEntity);
    }
}