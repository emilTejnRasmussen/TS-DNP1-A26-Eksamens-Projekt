using CLI.UI.Helpers;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageUsers;

public class AuthUser(IUserRepository userRepository)
{
    private readonly CreateUserView _createUserView = new(userRepository);

    public async Task<User> ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();

            ConsoleHelper.PrintHeader("Auth");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an [green]option[/]:")
                    .AddChoices("Login", "Create new account"
                    )
            );

            switch (choice)
            {
                case "Login":
                {
                    var user = await LoginAsync();

                    if (user is not null)
                        return user;

                    break;
                }

                case "Create new account":
                    return await _createUserView.ShowAsync();
            }
        }
    }

    private async Task<User?> LoginAsync()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new Rule("[bold]Login[/]")
                .LeftJustified()
        );

        AnsiConsole.WriteLine();

        var username = AnsiConsole.Ask<string>("Username:");

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Password:")
                .Secret()
        );

        var user = await userRepository.GetByUsernameAsync(username);


        if (user is not null && user.Password == password) return user;

        AnsiConsole.MarkupLine("[red]Invalid username or password.[/]");
        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");

        Console.ReadKey(true);

        return null;
    }
}