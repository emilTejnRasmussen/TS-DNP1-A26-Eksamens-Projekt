using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class SubforumInMemoryRepository : GenericInMemoryRepository<Subforum>, ISubforumRepository 
{
    public SubforumInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter().GetResult();
    }

    private async Task SeedDataAsync()
    {
        await AddAsync(new Subforum(
            "General",
            "General discussion about anything.",
            1));

        await AddAsync(new Subforum(
            "Programming",
            "Discuss programming, software development and technology.",
            1));

        await AddAsync(new Subforum(
            "Off Topic",
            "Everything that does not belong anywhere else.",
            2));
    }
}