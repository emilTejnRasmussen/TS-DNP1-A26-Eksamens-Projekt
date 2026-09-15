using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.Helpers;

public class ManagePostsView(
    IPostRepository postRepository)
{
    public async Task ShowAsync(int userId)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Manage Posts[/]")
                    .AddChoices(
                        "Update post",
                        "Delete post",
                        "<- Go back"
                    )
            );

            switch (choice)
            {
                case "Update post":
                    await UpdateAsync(userId);
                    break;

                case "Delete post":
                    await DeleteAsync(userId);
                    break;

                case "<- Go back":
                    return;
            }
        }
    }

    private async Task UpdateAsync(int userId)
    {
        var post = SelectPost(userId);

        if (post is null)
            return;

        AnsiConsole.Clear();

        var title = AnsiConsole.Prompt(
            new TextPrompt<string>("Title:")
                .DefaultValue(post.Title)
        );

        var body = AnsiConsole.Prompt(
            new TextPrompt<string>("Body:")
                .DefaultValue(post.Body)
        );

        post.Title = title;
        post.Body = body;

        await postRepository.UpdateAsync(post);

        AnsiConsole.MarkupLine("[green]Post updated.[/]");
        Console.ReadKey(true);
    }

    private async Task DeleteAsync(int userId)
    {
        var post = SelectPost(userId);

        if (post is null)
            return;

        if (!AnsiConsole.Confirm(
                $"Delete [red]{Markup.Escape(post.Title)}[/]?"))
            return;

        await postRepository.DeleteAsync(post.Id);

        AnsiConsole.MarkupLine("[green]Post deleted.[/]");
        Console.ReadKey(true);
    }

    private Post? SelectPost(int userId)
    {
        var posts = postRepository
            .GetMany()
            .Where(p => p.UserId == userId)
            .ToList();

        if (posts.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]You have no posts.[/]");
            Console.ReadKey(true);
            return null;
        }

        var choices = posts
            .Select(p => p.Id)
            .Append(0)
            .ToList();

        var selectedId = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("[bold]Select post[/]")
                .UseConverter(id =>
                {
                    if (id == 0)
                        return "[grey]<- Go back[/]";

                    return Markup.Escape(
                        posts.First(p => p.Id == id).Title);
                })
                .AddChoices(choices)
        );

        return selectedId == 0
            ? null
            : posts.First(p => p.Id == selectedId);
    }
}