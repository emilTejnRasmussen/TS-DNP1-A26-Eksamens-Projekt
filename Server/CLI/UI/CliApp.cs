using CLI.UI.Helpers;
using CLI.UI.ManageSubforums;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI;

public class CliApp(
    IUserRepository userRepository,
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    ISubforumRepository subforumRepository,
    ICommentVoteRepository commentVoteRepository,
    IPostVoteRepository postVoteRepository)
{
    private readonly AuthUser _authUser = new(userRepository);
    private readonly ViewSubforumsView _viewSubforumsView = new(
        subforumRepository, 
        postRepository, 
        commentRepository, 
        commentVoteRepository, 
        userRepository,
        postVoteRepository);

    private readonly ManageView _manageView = new(subforumRepository, postRepository, commentRepository);
    private readonly AccountView _accountView = new(userRepository);

    private User? _currentUser;

    public async Task StartAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();

            if (_currentUser is null)
            {
                _currentUser = await _authUser.ShowAsync();
                continue;
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Logged in as [green]{Markup.Escape(_currentUser.Username)}[/]")
                    .AddChoices(
                        "Browse subforums",
                        "Manage",
                        "My account",
                        "Log out",
                        "Quit"
                    )
            );

            switch (choice)
            {
                case "Browse subforums":
                    await _viewSubforumsView.ShowAsync(_currentUser.Id);
                    break;

                case "Manage":
                    await _manageView.ShowAsync(_currentUser.Id);
                    break;

                case "My account":
                    _currentUser = await _accountView.ShowAsync(_currentUser);
                    break;

                case "Log out":
                    _currentUser = null;
                    break;

                case "Quit":
                    return;
            }
        }
    }
}