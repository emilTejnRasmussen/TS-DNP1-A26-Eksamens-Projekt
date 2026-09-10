using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class UserInMemoryRepository : GenericInMemoryRepository<User>, IUserRepository
{
    private void SeedData()
    {
        AddAsync(new User("username1", "qwerty"));
        AddAsync(new User("username2", "qwerty"));
    }
}