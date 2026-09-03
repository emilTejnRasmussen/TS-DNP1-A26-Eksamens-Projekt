using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class PostInMemoryRepository : GenericInMemoryRepository<Post>, IPostRepository
{
    // I can do like this to add extra functionality to only this implementation
    // e.g. add methods in repo: IPostRepository
}