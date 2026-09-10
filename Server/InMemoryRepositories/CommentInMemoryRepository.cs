using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : GenericInMemoryRepository<Comment>, ICommentRepository
{
    private void SeedData()
    {
        AddAsync(new Comment("This is a comment", 1, 1, null));
        AddAsync(new Comment("This is also a comment", 2, 1, 1));
    }
}