using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class PostInMemoryRepository : GenericInMemoryRepository<Post>, IPostRepository
{
    // I can do like this to add extra functionality to only this implementation
    // e.g. add methods in repo: IPostRepository
    
    private void SeedData()
    {
        AddAsync(new Post("This is my post", "This is my opinion", 1, 1));
        AddAsync(new Post("This is also my post", "This is also my opinion", 1, 1));
    }
}