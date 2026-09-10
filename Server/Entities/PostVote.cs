namespace Entities;

public class PostVote(int userId, int postId, VoteType voteType) : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; } = userId;
    public int PostId { get; set; } = postId;
    public VoteType VoteType { get; set; } = voteType;
}