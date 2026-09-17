using Entities;
using RepositoryContract;

namespace FileRepositories;

public class PostFileRepository : GenericFileRepository<Post>, IPostRepository
{
    public async Task<int> CountBySubforumIdAsync(int subforumId)
    {
        var entities = await ReadAllEntities();
        var count = entities.Count(p => p.SubforumId == subforumId);
        
        return count;
    }
}