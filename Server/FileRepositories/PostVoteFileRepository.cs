using Entities;
using RepositoryContract;

namespace FileRepositories;

public class PostVoteFileRepository : GenericFileRepository<PostVote>, IPostVoteRepository
{
    public async Task<int> CountByPostIdAndVoteTypeAsync(int postId, VoteType voteType)
    {
        var entities = await ReadAllEntities();
        var result = entities.Count(vote => vote.PostId == postId && vote.VoteType == voteType);
        
        return result;
    }

    public async Task<PostVote?> GetFromUserIdAndPostIdAsync(int userId, int postId)
    {
        var entities = await ReadAllEntities();
        var result = entities.FirstOrDefault(pv => pv.UserId == userId && pv.PostId == postId);
        
        return result;
    }
}