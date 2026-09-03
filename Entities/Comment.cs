namespace Entities;

public class Comment(string body, int userId, int postId, int? parentCommentId) : IEntity
{
    public int Id { get; set; }
    public string Body { get; set; } = body;

    public int UserId { get; set; } = userId;
    public int PostId { get; set; } = postId;
    public int? ParentCommentId { get; set; } = parentCommentId;
}