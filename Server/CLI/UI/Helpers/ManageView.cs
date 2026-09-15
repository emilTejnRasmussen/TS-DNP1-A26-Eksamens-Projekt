using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageSubforums;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.Helpers;

public class ManageView(
    ISubforumRepository subforumRepository,
    IPostRepository postRepository,
    ICommentRepository commentRepository)
{
    private readonly ManageSubforumsView _manageSubforumsView = new ManageSubforumsView(subforumRepository);
    private readonly ManagePostsView _managePostsView = new ManagePostsView(postRepository);
    private readonly ManageCommentsView _manageCommentsView = new ManageCommentsView(commentRepository);

    public async Task ShowAsync(int userId)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Manage[/]")
                    .AddChoices(
                        "Subforums",
                        "Posts",
                        "Comments",
                        "<- Go back"
                    )
            );

            switch (choice)
            {
                case "Subforums":
                    await _manageSubforumsView.ShowAsync(userId);
                    break;

                case "Posts":
                    await _managePostsView.ShowAsync(userId);
                    break;

                case "Comments":
                    await _manageCommentsView.ShowAsync(userId);
                    break;

                case "<- Go back":
                    return;
            }
        }
    }
}