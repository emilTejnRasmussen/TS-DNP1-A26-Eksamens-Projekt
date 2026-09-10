using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class UserInMemoryRepository : GenericInMemoryRepository<User>, IUserRepository
{
    public UserInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter().GetResult();
    }

    private async Task SeedDataAsync()
    {
        await AddAsync(new User("joe", "1234"));
        await AddAsync(new User("alice", "1234"));
        await AddAsync(new User("bob", "1234"));
    }
}