using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class UserInMemoryRepository : GenericInMemoryRepository<User>, IUserRepository
{
    public UserInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter().GetResult();
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        var result = _entities.FirstOrDefault(u => u.Username == username);
        return Task.FromResult(result);
    }

    private async Task SeedDataAsync()
    {
        await AddAsync(new User("joe", "1234"));
        await AddAsync(new User("alice", "1234"));
        await AddAsync(new User("bob", "1234"));
    }
}