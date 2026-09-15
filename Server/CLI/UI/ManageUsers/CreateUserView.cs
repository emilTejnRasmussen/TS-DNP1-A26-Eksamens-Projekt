using CLI.UI.Helpers;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository)
{
    public async Task<User> ShowAsync()
    {
        AnsiConsole.Clear();

        ConsoleHelper.PrintHeader("Create User");

        string username;

        while (true)
        {
            username = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter a [green]username[/]:")
                    .Validate(value =>
                        string.IsNullOrWhiteSpace(value)
                            ? ValidationResult.Error("[red]Username cannot be empty[/]")
                            : ValidationResult.Success())
            );

            var existingUser = await userRepository.GetByUsernameAsync(username);

            if (existingUser is null) break;

            AnsiConsole.MarkupLine("[red]That username is already taken.[/]");
        }

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter a [green]password[/]:")
                .Secret()
                .Validate(value =>
                    string.IsNullOrWhiteSpace(value)
                        ? ValidationResult.Error("[red]Password cannot be empty[/]")
                        : ValidationResult.Success())
        );

        var user = new User(username, password);
        var created = await userRepository.AddAsync(user);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[green]Account created.[/] Welcome, [blue]{Markup.Escape(created.Username)}[/]!");

        return created;
    }
}