using Entities;
using RepositoryContract;

namespace Services;

public class PostVoteService(
    IPostRepository postRepo,
    IPostVoteRepository postVoteRepo) : IPostVoteService
{
    public async Task SetVoteAsync(int postId, int userId, VoteType voteType)
    {
        await postRepo.GetSingleAsync(postId);

        var existingVote = postVoteRepo.GetMany()
            .SingleOrDefault(v =>
                v.PostId == postId &&
                v.UserId == userId);
        
        if (existingVote is null)
        {
            var vote = new PostVote(
                userId,
                postId,
                voteType
            );

            await postVoteRepo.AddAsync(vote);
            return;
        }
        
        if (existingVote.VoteType == voteType)
        {
            await postVoteRepo.DeleteAsync(existingVote.Id);
            return;
        }
        
        existingVote.VoteType = voteType;

        await postVoteRepo.UpdateAsync(existingVote);
    }
}