using Entities;
using RepositoryContract;

namespace FileRepositories;

public class UserFileRepository : GenericFileRepository<User>, IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username)
    {
        var entities = await ReadAllEntities();
        var result = entities.FirstOrDefault(u => u.Username == username);
        return result;
    }
}