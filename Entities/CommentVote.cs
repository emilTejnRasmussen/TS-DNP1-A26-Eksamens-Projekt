namespace Entities;

public class CommentVote(int id, int userId, int commentId, VoteType voteType)
{
    public int Id { get; set; } = id;
    public int UserId { get; set; } = userId;
    public int CommentId { get; set; } = commentId;
    public VoteType VoteType { get; set; } = voteType;
}