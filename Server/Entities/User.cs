namespace Entities;

public class User(string username) : IEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = username;
    public string hashedPassword { get; set; }
}