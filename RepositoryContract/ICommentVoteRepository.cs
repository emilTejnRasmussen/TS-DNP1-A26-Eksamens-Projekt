using Entities;

namespace RepositoryContract;

public interface ICommentVoteRepository
{
    Task<CommentVote> AddAsync(CommentVote commentVote);
    Task UpdateAsync(CommentVote commentVote);
    Task DeleteAsync(int id);
    Task<CommentVote> GetSingleAsync(int id);
    IQueryable<CommentVote> GetManyAsync();   
}