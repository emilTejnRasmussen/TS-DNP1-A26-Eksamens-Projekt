namespace Entities;

public class Subforum(int id, string name, string description, int creatorId) : IEntity
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public int CreatorId { get; set; } = creatorId;
}