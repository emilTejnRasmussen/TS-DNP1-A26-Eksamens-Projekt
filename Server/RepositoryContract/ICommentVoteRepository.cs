using Entities;

namespace RepositoryContract;

public interface ICommentVoteRepository : IRepository<CommentVote>
{
    Task<CommentVote?> GetFromUserIdAndCommentIdAsync(int userId, int commentId);
    Task<int> CountByCommentIdAndVoteTypeAsync(int commentId, VoteType voteType);
}