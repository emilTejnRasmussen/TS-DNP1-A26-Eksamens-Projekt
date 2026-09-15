using CLI.UI.ManageSubforums;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.Helpers;

public class ManageSubforumsView(
    ISubforumRepository subforumRepository)
{
    private readonly CreateSubforumView _createSubforumView =
        new(subforumRepository);

    public async Task ShowAsync(int userId)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Manage Subforums[/]")
                    .AddChoices(
                        "Create subforum",
                        "Update subforum",
                        "Delete subforum",
                        "<- Go back"
                    )
            );

            switch (choice)
            {
                case "Create subforum":
                    await _createSubforumView.ShowAsync(userId);
                    break;

                case "Update subforum":
                    await UpdateAsync(userId);
                    break;

                case "Delete subforum":
                    await DeleteAsync(userId);
                    break;

                case "<- Go back":
                    return;
            }
        }
    }

    private async Task UpdateAsync(int userId)
    {
        var subforum = SelectSubforum(userId);

        if (subforum is null)
            return;

        AnsiConsole.Clear();

        var name = AnsiConsole.Prompt(
            new TextPrompt<string>("Name:")
                .DefaultValue(subforum.Name)
        );

        var description = AnsiConsole.Prompt(
            new TextPrompt<string>("Description:")
                .DefaultValue(subforum.Description)
        );

        subforum.Name = name;
        subforum.Description = description;

        await subforumRepository.UpdateAsync(subforum);

        AnsiConsole.MarkupLine("[green]Subforum updated.[/]");
        Console.ReadKey(true);
    }

    private async Task DeleteAsync(int userId)
    {
        var subforum = SelectSubforum(userId);

        if (subforum is null)
            return;

        var confirmed = AnsiConsole.Confirm(
            $"Delete [red]{Markup.Escape(subforum.Name)}[/]?"
        );

        if (!confirmed)
            return;

        await subforumRepository.DeleteAsync(subforum.Id);

        AnsiConsole.MarkupLine("[green]Subforum deleted.[/]");
        Console.ReadKey(true);
    }

    private Subforum? SelectSubforum(int userId)
    {
        var subforums = subforumRepository
            .GetMany()
            .Where(s => s.CreatorId == userId)
            .ToList();

        if (subforums.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]You have no subforums.[/]");
            Console.ReadKey(true);
            return null;
        }

        var choices = subforums
            .Select(s => s.Id)
            .Append(0)
            .ToList();

        var selectedId = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("[bold]Select subforum[/]")
                .UseConverter(id =>
                {
                    if (id == 0)
                        return "[grey]<- Go back[/]";

                    var subforum =
                        subforums.First(s => s.Id == id);

                    return Markup.Escape(subforum.Name);
                })
                .AddChoices(choices)
        );

        return selectedId == 0
            ? null
            : subforums.First(s => s.Id == selectedId);
    }
}