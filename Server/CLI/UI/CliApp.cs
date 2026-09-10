using CLI.UI.Helpers;
using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageSubforums;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContract;

namespace CLI.UI;

public class CliApp(
    IUserRepository userRepository,
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    ISubforumRepository subforumRepository)
{
    private readonly CreateUserView _createUserView = new(userRepository);
    private readonly CreateSubforumView _createSubforumView = new(subforumRepository);
    private readonly ViewSubforumsView _viewSubforumsView = new(subforumRepository, postRepository);

    private User? _currentUser;

    public async Task StartAsync()
    {
        var running = true;
        while (running)
        {
            if (_currentUser is null)
            {
               _currentUser = await _createUserView.ShowAsync();
               continue;
            }

            var option = PrintMenu();

            switch (option)
            {
                case 1:
                    await _viewSubforumsView.ShowAsync();
                    break;
                case 2:
                    await _createSubforumView.ShowAsync(_currentUser.Id);
                    break;
                case 0:
                    running = false;
                    break;
            }
        }
    }

    private int PrintMenu()
    {
        ConsoleHelper.PrintHeader("Forum");
        Console.WriteLine("1. View subforums");
        Console.WriteLine("2. Create subforum");
        Console.WriteLine();
        Console.WriteLine("0. Exit program");

        return ConsoleHelper.ReadInt("Enter option: ");
    }
}