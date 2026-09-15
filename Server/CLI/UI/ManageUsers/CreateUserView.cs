using CLI.UI.Helpers;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository)
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<User> ShowAsync()
    {
        AnsiConsole.Clear();
        
        ConsoleHelper.PrintHeader("Create User");
        var username = AnsiConsole.Ask<string>("Enter a [green]username[/]:");
        var password = AnsiConsole.Ask<string>("Enter a [green]password[/]:");

        User user = new(username, password);

        var created = await userRepository.AddAsync(user);
        
        AnsiConsole.MarkupLine($"Created User, [blue]{username}[/] with ID {created.Id}");

        return created;
    }
}