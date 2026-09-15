using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : GenericInMemoryRepository<Comment>, ICommentRepository
{
    public CommentInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter().GetResult();
    }

    public Task<int> CountByParentCommentIdAsync(int parentCommentId)
    {
        var count = _entities.Count(comment => comment.ParentCommentId == parentCommentId);

        return Task.FromResult(count);
    }

    private async Task SeedDataAsync()
    {
        await AddAsync(new Comment(
            "Hello! Nice to be here.",
            2,
            1,
            null));

        await AddAsync(new Comment(
            "Welcome!",
            1,
            1,
            1));

        await AddAsync(new Comment(
            "I am currently working on a C# project.",
            3,
            2,
            null));

        await AddAsync(new Comment(
            "Async confused me at first too.",
            2,
            3,
            null));

        await AddAsync(new Comment(
            "Java is still my favourite.",
            1,
            4,
            null));
    }
}