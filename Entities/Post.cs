namespace Entities;

public class Post(int id, string title, string body, int userId, int subforumId) : IEntity
{
    public int Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Body { get; set; } = body;

    public int UserId { get; set; } = userId;
    public int SubforumId { get; set; } = subforumId;
}