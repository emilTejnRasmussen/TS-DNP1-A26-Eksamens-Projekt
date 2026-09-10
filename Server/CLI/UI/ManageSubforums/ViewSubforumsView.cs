using CLI.UI.Helpers;
using CLI.UI.ManagePosts;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManageSubforums;

public class ViewSubforumsView(
    ISubforumRepository subforumRepository, 
    IPostRepository postRepository, 
    ICommentRepository commentRepository,
    ICommentVoteRepository commentVoteRepository)
{
    private readonly CreatePostView _createPostView = new(postRepository);
    private readonly ViewPostsView _viewPostsView = new(postRepository, commentRepository, commentVoteRepository);

    public async Task ShowAsync(int userId)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Subforums");

            var subforums = subforumRepository.GetMany().ToList();
            var subforum = ConsoleHelper.SelectFromList(subforums, s => s.Name);

            if (subforum is null) return;

            await DisplaySubforumAsync(subforum, userId);
        }
 
    }

    private async Task DisplaySubforumAsync(Subforum subforum, int userId)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader(subforum.Name);

            Console.WriteLine("1. View posts");
            Console.WriteLine("2. Create post");
            Console.WriteLine();
            Console.WriteLine("0. Go back");

            var option = ConsoleHelper.ReadInt("Select option: ", 0, 2);

            switch (option)
            {
                case 1:
                    await _viewPostsView.ShowAsync(userId, subforum);
                    break;

                case 2:
                    await _createPostView.ShowAsync(userId, subforum.Id);
                    break;

                case 0:
                    return;
            }
        }
    }
}