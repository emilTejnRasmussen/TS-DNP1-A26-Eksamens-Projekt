namespace Entities;

public class Comment(int id, string body, int userId, int postId, int? parentCommentId)
{
    public int Id { get; set; } = id;
    public string Body { get; set; } = body;

    public int UserId { get; set; } = userId;
    public int PostId { get; set; } = postId;
    public int? ParentCommentId { get; set; } = parentCommentId;
}