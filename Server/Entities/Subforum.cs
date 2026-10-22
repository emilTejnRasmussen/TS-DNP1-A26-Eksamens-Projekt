namespace Entities;

public class Subforum(
    string name, 
    string description, 
    int creatorId,
    string color,
    string symbol) : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public int CreatorId { get; set; } = creatorId;
    
    public string Color { get; set; } = color;
    
    public string Symbol { get; set; } = symbol;
}