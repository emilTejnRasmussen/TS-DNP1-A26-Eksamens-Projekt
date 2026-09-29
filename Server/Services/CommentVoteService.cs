using DTO.CommentVote;
using Entities;
using RepositoryContract;
using Services.Interfaces;

namespace Services;

public class CommentVoteService(
    ICommentRepository commentRepo,
    ICommentVoteRepository commentVoteRepo) : ICommentVoteService
{
    public async Task<CommentVoteSummaryDto> GetVotesAsync(int commentId)
    {
        await commentRepo.GetSingleAsync(commentId);

        var votes = commentVoteRepo.GetMany()
            .Where(v => v.CommentId == commentId)
            .ToList();

        return new CommentVoteSummaryDto(
            votes.Count(v => v.VoteType == VoteType.Like),
            votes.Count(v => v.VoteType == VoteType.Dislike)
        );
    }

    public async Task SetVoteAsync(int commentId, int userId, VoteType voteType)
    {
        await commentRepo.GetSingleAsync(commentId);

        var existingVote = commentVoteRepo.GetMany()
            .SingleOrDefault(v =>
                v.CommentId == commentId &&
                v.UserId == userId);

        if (existingVote is null)
        {
            var vote = new CommentVote(
                userId,
                commentId,
                voteType
            );

            await commentVoteRepo.AddAsync(vote);
            return;
        }

        if (existingVote.VoteType == voteType)
        {
            await commentVoteRepo.DeleteAsync(existingVote.Id);
            return;
        }

        existingVote.VoteType = voteType;

        await commentVoteRepo.UpdateAsync(existingVote);
    }
}