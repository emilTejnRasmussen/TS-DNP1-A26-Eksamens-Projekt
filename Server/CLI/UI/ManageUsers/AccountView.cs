using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageUsers;

public class AccountView(IUserRepository userRepository)
{
    public async Task<User?> ShowAsync(User user)
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine(
                $"[bold]{Markup.Escape(user.Username)}[/]"
            );

            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]My Account[/]")
                    .AddChoices(
                        "View details",
                        "Update username",
                        "Update password",
                        "Delete account",
                        "<- Go back"
                    )
            );

            switch (choice)
            {
                case "View details":
                    ShowDetails(user);
                    break;

                case "Update username":
                    user = await UpdateUsernameAsync(user);
                    break;

                case "Update password":
                    user = await UpdatePasswordAsync(user);
                    break;

                case "Delete account":
                    if (await DeleteAsync(user))
                        return null;
                    break;

                case "<- Go back":
                    return user;
            }
        }
    }

    private static void ShowDetails(User user)
    {
        AnsiConsole.Clear();

        var table = new Table();

        table.AddColumn("ID");
        table.AddColumn("Username");
        table.AddRow(user.Id.ToString(), Markup.Escape(user.Username));

        AnsiConsole.Write(table);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    private async Task<User> UpdateUsernameAsync(User user)
    {
        AnsiConsole.Clear();

        var username = AnsiConsole.Prompt(
            new TextPrompt<string>("New username:")
                .DefaultValue(user.Username)
                .Validate(username =>
                    string.IsNullOrWhiteSpace(username)
                        ? ValidationResult.Error(
                            "[red]Username cannot be empty[/]")
                        : ValidationResult.Success())
        );

        user.Username = username;

        await userRepository.UpdateAsync(user);

        AnsiConsole.MarkupLine("[green]Username updated.[/]");
        Console.ReadKey(true);

        return user;
    }

    private async Task<User> UpdatePasswordAsync(User user)
    {
        AnsiConsole.Clear();

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("New password:")
                .Secret()
                .Validate(password =>
                    string.IsNullOrWhiteSpace(password)
                        ? ValidationResult.Error(
                            "[red]Password cannot be empty[/]")
                        : ValidationResult.Success())
        );

        user.Password = password;

        await userRepository.UpdateAsync(user);

        AnsiConsole.MarkupLine("[green]Password updated.[/]");
        Console.ReadKey(true);

        return user;
    }

    private async Task<bool> DeleteAsync(User user)
    {
        var confirmed = AnsiConsole.Confirm(
            $"Are you sure you want to delete " + $"[red]{Markup.Escape(user.Username)}[/]?"
        );

        if (!confirmed)
            return false;

        await userRepository.DeleteAsync(user.Id);

        AnsiConsole.MarkupLine("[green]Account deleted.[/]");
        Console.ReadKey(true);

        return true;
    }
}