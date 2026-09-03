using Entities;
using RepositoryContract;

namespace InMemoryRepositories;

public class GenericInMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _entities = [];
    
    public Task<T> AddAsync(T entity)
    {
        entity.Id = _entities.Count != 0
            ? _entities.Max(e => e.Id + 1)
            : 1;
        
        _entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(T entity)
    {
        var existingEntity = _entities.SingleOrDefault(e => e.Id == entity.Id);
        if (existingEntity is null)
        {
            throw new InvalidOperationException($"{typeof(T).Name} with ID '{entity.Id}' not found");
        }

        _entities.Remove(existingEntity);
        _entities.Add(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var entityToDelete = _entities.SingleOrDefault(e => e.Id == id);
        if (entityToDelete is null)
        {
            throw new InvalidOperationException($"{typeof(T).Name} with ID '{id}' not found");
        }

        _entities.Remove(entityToDelete);
        return Task.CompletedTask;
    }

    public Task<T> GetSingleAsync(int id)
    {
        var existingEntity = _entities.SingleOrDefault(e => e.Id == id);
        return existingEntity is null 
            ? throw new InvalidOperationException($"{typeof(T).Name} with ID '{id}' not found") 
            : Task.FromResult(existingEntity);
    }

    public IQueryable<T> GetManyAsync()
    {
        return _entities.AsQueryable();
    }
}