using CLI.UI.Helpers;
using CLI.UI.ManageComments;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManagePosts;

public class ViewPostsView(IPostRepository postRepository, ICommentRepository commentRepository, ICommentVoteRepository commentVoteRepository)
{
    private readonly CreateCommentView _createCommentView = new(commentRepository);
    private readonly ViewCommentView _viewCommentView = new(commentRepository, commentVoteRepository);
    
    public async Task ShowAsync(int userId, Subforum subforum)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader(subforum.Name);
            Console.WriteLine(subforum.Description);

            ConsoleHelper.PrintDivider();

            var posts = postRepository
                .GetMany()
                .Where(p => p.SubforumId == subforum.Id)
                .ToList();
            
            var post = ConsoleHelper.SelectFromList(posts, p => p.Title);
            
            if (post is null) return;

            await DisplayPostAsync(subforum, post, userId);
        }    
    }

    private async Task DisplayPostAsync(Subforum subforum, Post post, int userId)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader(post.Title);
            Console.WriteLine("1. View comments");
            Console.WriteLine("2. Create comment");
            Console.WriteLine();
            Console.WriteLine("0. Go back");

            var option = ConsoleHelper.ReadInt("Select option: ", 0, 2);

            switch (option)
            {
                case 1:
                    await _viewCommentView.ShowAsync(userId, subforum, post);
                    break;

                case 2:
                    await _createCommentView.ShowAsync(userId, post.Id);
                    break;

                case 0:
                    return;
            }
        }
    }
}