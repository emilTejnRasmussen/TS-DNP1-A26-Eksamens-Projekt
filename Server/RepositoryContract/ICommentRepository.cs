using Entities;

namespace RepositoryContract;

public interface ICommentRepository : IRepository<Comment>
{
    Task<int> CountByParentCommentIdAsync(int parentCommentId);
    Task<int> CountByPostIdAsync(int postId);
}