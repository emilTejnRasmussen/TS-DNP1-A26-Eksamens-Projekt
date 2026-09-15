using Entities;

namespace RepositoryContract;

public interface IPostRepository : IRepository<Post>
{
    Task<int> CountBySubforumIdAsync(int subforumId);
}