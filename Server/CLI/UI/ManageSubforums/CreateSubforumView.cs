using CLI.UI.Helpers;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageSubforums;

public class CreateSubforumView(ISubforumRepository subforumRepository)
{
    public async Task ShowAsync(int creatorId)
    {
        AnsiConsole.Clear();
        ConsoleHelper.PrintHeader("Create post");
        AnsiConsole.WriteLine();

        var name = AnsiConsole.Prompt(
            new TextPrompt<string>("Name:")
                .Validate(title =>
                    string.IsNullOrWhiteSpace(title)
                        ? ValidationResult.Error("[red]Name cannot be empty[/]")
                        : ValidationResult.Success())
        );

        var description = AnsiConsole.Prompt(
            new TextPrompt<string>("Description:")
                .Validate(body =>
                    string.IsNullOrWhiteSpace(body)
                        ? ValidationResult.Error("[red]Description cannot be empty[/]")
                        : ValidationResult.Success())
        );

        var subforum = new Subforum(name, description, creatorId);
        var createdSubforum = await subforumRepository.AddAsync(subforum);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"Subforum [bold]{Markup.Escape(createdSubforum.Name)}[/] [green]created[/].");

        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}