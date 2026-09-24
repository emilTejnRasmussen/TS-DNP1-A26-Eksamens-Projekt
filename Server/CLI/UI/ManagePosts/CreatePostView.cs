using CLI.UI.Helpers;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IPostRepository postRepository)
{
    public async Task ShowAsync(int userId, int subforumId)
    {
        AnsiConsole.Clear();

        ConsoleHelper.PrintHeader("Create post");

        AnsiConsole.WriteLine();

        var title = AnsiConsole.Prompt(
            new TextPrompt<string>("Title:")
                .Validate(title =>
                    string.IsNullOrWhiteSpace(title)
                        ? ValidationResult.Error("[red]Title cannot be empty[/]")
                        : ValidationResult.Success())
        );

        var body = AnsiConsole.Prompt(
            new TextPrompt<string>("Body:")
                .Validate(body =>
                    string.IsNullOrWhiteSpace(body)
                        ? ValidationResult.Error("[red]Body cannot be empty[/]")
                        : ValidationResult.Success())
        );

        var post = new Post(title, body, userId, subforumId);

        var createdPost = await postRepository.AddAsync(post);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"Post [bold]{Markup.Escape(createdPost.Title)}[/] [green]created[/].");

        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}