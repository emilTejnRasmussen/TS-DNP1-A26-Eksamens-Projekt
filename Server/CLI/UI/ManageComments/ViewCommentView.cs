using CLI.UI.Helpers;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManageComments;

public class ViewCommentView(ICommentRepository commentRepository)
{
    public void ShowAsync(int userId, Subforum subforum, Post post)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader(subforum.Name);
            Console.WriteLine(post.Title);

            ConsoleHelper.PrintDivider();

            var comments = commentRepository
                .GetMany()
                .Where(c => c.PostId == post.Id)
                .ToList();

            var comment = ConsoleHelper.SelectFromList(comments, c => c.Body);

            if (comment is null) return;

            DisplayCommentAsync(comment, userId);
        }
    }

    private void DisplayCommentAsync(Comment comment, int userId)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader(comment.Body);

            Console.WriteLine("1. Like comment");
            Console.WriteLine("2. Dislike comment");
            Console.WriteLine();
            Console.WriteLine("0. Go back");

            var option = ConsoleHelper.ReadInt("Select option: ", 0, 2);

            switch (option)
            {
                case 1:
                    Console.WriteLine($"user {userId} liked this comment");
                    break;

                case 2:
                    Console.WriteLine($"user {userId} disliked this comment");
                    break;

                case 0:
                    return;
            }
        }
    }
}