using Entities;
using RepositoryContract;

namespace FileRepositories;

public class UserFileRepository : GenericFileRepository<User>, IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        var entities = await ReadAllEntities();
        return entities.FirstOrDefault(u => u.Username == username);
    }

    public async Task<bool> UsernameAlreadyExistsAsync(string username)
    {
        var entities = await ReadAllEntities();
        return entities.Any(u => u.Username == username);
    }
}