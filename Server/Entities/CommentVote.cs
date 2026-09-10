namespace Entities;

public class CommentVote(int userId, int commentId, VoteType voteType) : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; } = userId;
    public int CommentId { get; set; } = commentId;
    public VoteType VoteType { get; set; } = voteType;
}