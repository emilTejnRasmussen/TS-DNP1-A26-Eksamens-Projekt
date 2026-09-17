using System.Net.Http.Headers;
using System.Text.Json;
using Entities;
using RepositoryContract;

namespace FileRepositories;

public class GenericFileRepository<T> : IRepository<T> where T : IEntity
{
    private readonly string _filepath = "data/" + typeof(T).Name + ".json";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };
    
    public GenericFileRepository()
    {
        Directory.CreateDirectory("data");
        
        if (!File.Exists(_filepath))
        {
            File.WriteAllText(_filepath, "[]");
        }
    }

    public async Task<T> AddAsync(T entity)
    {
        var entities = await ReadAllEntities();

        entity.Id = entities.Count != 0
            ? entities.Max(e => e.Id) + 1
            : 1;
        
        entities.Add(entity);
        
        await WriteAllEntities(entities);

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        var entities = await ReadAllEntities();

        var existingEntity = entities.SingleOrDefault(entity);
        
        if (existingEntity is null)
        {
            throw new InvalidOperationException($"{typeof(T).Name} with ID '{entity.Id}' not found");
        }

        entities.Remove(existingEntity);
        entities.Add(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entities = await ReadAllEntities();
           
        var entityToDelete = entities.SingleOrDefault(e => e.Id == id);
        
        if (entityToDelete is null)
        {
            throw new InvalidOperationException($"{typeof(T).Name} with ID '{id}' not found");
        }

        entities.Remove(entityToDelete);
    }

    public async Task<T> GetSingleAsync(int id)
    {
        var entities = await ReadAllEntities();
        
        var existingEntity = entities.SingleOrDefault(e => e.Id == id);

        return existingEntity 
               ?? throw new InvalidOperationException($"{typeof(T).Name} with ID '{id}' not found");
    }

    public IQueryable<T> GetMany()
    {
        var entities = ReadAllEntities().Result;
        return entities.AsQueryable();
    }

    protected async Task<List<T>> ReadAllEntities()
    {
        var entityAsJson = await File.ReadAllTextAsync(_filepath);
        var entities = JsonSerializer.Deserialize<List<T>>(entityAsJson);

        return entities ?? throw new InvalidDataException("Deserialized data was null.");
    }

    private async Task WriteAllEntities(List<T> entities)
    {
        var entityAsJson = JsonSerializer.Serialize(entities, JsonOptions);

        await File.WriteAllTextAsync(_filepath, entityAsJson);
    }
}