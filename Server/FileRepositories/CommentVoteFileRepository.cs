using Entities;
using RepositoryContract;

namespace FileRepositories;

public class CommentVoteFileRepository : GenericFileRepository<CommentVote>, ICommentVoteRepository
{
    public async Task<CommentVote?> GetFromUserIdAndCommentIdAsync(int userId, int commentId)
    {
        var entities = await ReadAllEntities();
        var existingEntity = entities.SingleOrDefault(cv => cv.UserId == userId && cv.CommentId == commentId);
        
        return existingEntity;
    }

    public async Task<int> CountByCommentIdAndVoteTypeAsync(int commentId, VoteType voteType)
    {
        var entities = await ReadAllEntities();
        var result = entities.Count(vote => vote.CommentId == commentId && vote.VoteType == voteType);
        return result;
    }
}