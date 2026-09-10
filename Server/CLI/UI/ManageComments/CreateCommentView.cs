using CLI.UI.Helpers;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManageComments;

public class CreateCommentView(ICommentRepository commentRepository)
{
    public async Task ShowAsync(int userId, int postId, int? parentCommentId = null)
    {
        ConsoleHelper.PrintHeader("Create comment");

        Console.WriteLine("Enter comment: ");
        var commentBody = Console.ReadLine() ?? "";

        Comment comment = new(commentBody, userId, postId, parentCommentId);

        await commentRepository.AddAsync(comment);
    }
}