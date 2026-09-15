using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class PostInMemoryRepository : GenericInMemoryRepository<Post>, IPostRepository
{
    public PostInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter().GetResult();
    }

    public Task<int> CountBySubforumIdAsync(int subforumId)
    {
        var count = _entities.Count(p => p.SubforumId == subforumId);
        return Task.FromResult(count);
    }

    private async Task SeedDataAsync()
    {
        await AddAsync(new Post(
            "Welcome to the forum",
            "Feel free to introduce yourself!",
            1,
            1));

        await AddAsync(new Post(
            "What are you working on?",
            "Share what you are currently building.",
            2,
            1));

        await AddAsync(new Post(
            "C# async and await",
            "I am trying to understand asynchronous programming in C#.",
            1,
            2));

        await AddAsync(new Post(
            "Favourite programming language",
            "What is your favourite programming language and why?",
            3,
            2));

        await AddAsync(new Post(
            "Weekend plans",
            "Anyone doing anything interesting this weekend?",
            2,
            3));
    }
}