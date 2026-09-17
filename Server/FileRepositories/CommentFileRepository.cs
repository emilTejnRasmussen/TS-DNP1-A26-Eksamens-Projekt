using Entities;
using RepositoryContract;

namespace FileRepositories;

public class CommentFileRepository : GenericFileRepository<Comment>, ICommentRepository
{
    public async Task<int> CountByParentCommentIdAsync(int parentCommentId)
    {
        var entities = await ReadAllEntities();
        return entities.Count(comment => comment.ParentCommentId == parentCommentId);
    }

    public async Task<int> CountByPostIdAsync(int postId)
    {
        var entities = await ReadAllEntities();
        return entities.Count(comment => comment.PostId == postId);
    }
}