using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.Helpers;

public class ManageCommentsView(
    ICommentRepository commentRepository)
{
    public async Task ShowAsync(int userId)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Manage Comments[/]")
                    .AddChoices(
                        "Update comment",
                        "Delete comment",
                        "<- Go back"
                    )
            );

            switch (choice)
            {
                case "Update comment":
                    await UpdateAsync(userId);
                    break;

                case "Delete comment":
                    await DeleteAsync(userId);
                    break;

                case "<- Go back":
                    return;
            }
        }
    }

    private async Task UpdateAsync(int userId)
    {
        var comment = SelectComment(userId);

        if (comment is null)
            return;

        AnsiConsole.Clear();

        var body = AnsiConsole.Prompt(
            new TextPrompt<string>("Comment:")
                .DefaultValue(comment.Body)
        );

        comment.Body = body;

        await commentRepository.UpdateAsync(comment);

        AnsiConsole.MarkupLine("[green]Comment updated.[/]");
        Console.ReadKey(true);
    }

    private async Task DeleteAsync(int userId)
    {
        var comment = SelectComment(userId);

        if (comment is null)
            return;

        if (!AnsiConsole.Confirm("Delete this comment?"))
            return;

        await commentRepository.DeleteAsync(comment.Id);

        AnsiConsole.MarkupLine("[green]Comment deleted.[/]");
        Console.ReadKey(true);
    }

    private Comment? SelectComment(int userId)
    {
        var comments = commentRepository
            .GetMany()
            .Where(c => c.UserId == userId)
            .ToList();

        if (comments.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]You have no comments.[/]");
            Console.ReadKey(true);
            return null;
        }

        var choices = comments
            .Select(c => c.Id)
            .Append(0)
            .ToList();

        var selectedId = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("[bold]Select comment[/]")
                .PageSize(10)
                .UseConverter(id =>
                {
                    if (id == 0)
                        return "[grey]<- Go back[/]";

                    var comment =
                        comments.First(c => c.Id == id);

                    return Markup.Escape(comment.Body);
                })
                .AddChoices(choices)
        );

        return selectedId == 0
            ? null
            : comments.First(c => c.Id == selectedId);
    }
}