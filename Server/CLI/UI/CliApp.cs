using CLI.UI.Helpers;
using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
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
    ICommentVoteRepository commentVoteRepository)
{
    private readonly AuthUser _authUser = new(userRepository);
    private readonly CreateSubforumView _createSubforumView = new(subforumRepository);
    private readonly ViewSubforumsView _viewSubforumsView = new(
        subforumRepository, 
        postRepository, 
        commentRepository, 
        commentVoteRepository, 
        userRepository);

    private User? _currentUser;

    public async Task StartAsync()
    {
        var running = true;
        while (running)
        {
            AnsiConsole.Clear();
            
            if (_currentUser is null)
            {
               _currentUser = await _authUser.ShowAsync();
               continue;
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an [green]option[/]:")
                    .AddChoices("View subforums", "Create new subforum", "Update a subforum", "Delete a subforum", "Quit"));
  
            // AnsiConsole.MarkupLine($"Deploying to [blue]{choice}[/]");

            switch (choice)
            {
                case "View subforums":
                    await _viewSubforumsView.ShowAsync(_currentUser.Id);
                    break;
                case "Create new subforum":
                    await _createSubforumView.ShowAsync(_currentUser.Id);
                    break;
                case "Update a subforum":
                    Console.WriteLine("Not implemenmted");
                    break;
                case "Delete a subforum":
                    Console.WriteLine("Not implemenmted!");
                    break;
                case "Quit":
                    running = false;
                    break;
            }
        }
    }
}