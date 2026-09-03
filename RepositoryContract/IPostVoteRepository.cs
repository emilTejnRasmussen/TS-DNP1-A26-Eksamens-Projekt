using Entities;

namespace RepositoryContract;

public interface IPostVoteRepository
{
    Task<PostVote> AddAsync(PostVote postVote);
    Task UpdateAsync(PostVote postVote);
    Task DeleteAsync(int id);
    Task<PostVote> GetSingleAsync(int id);
    IQueryable<PostVote> GetManyAsync();
}