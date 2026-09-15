using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageComments;

public class CreateCommentView(ICommentRepository commentRepository)
{
    public async Task ShowAsync(
        int userId,
        int postId,
        int? parentCommentId = null)
    {
        AnsiConsole.Clear();

        var title = parentCommentId is null
            ? "Create Comment"
            : "Create Reply";

        AnsiConsole.Write(
            new Rule($"[bold]{title}[/]")
                .LeftJustified()
        );

        AnsiConsole.WriteLine();

        var body = AnsiConsole.Prompt(
            new TextPrompt<string>("Comment:")
                .Validate(comment =>
                    string.IsNullOrWhiteSpace(comment)
                        ? ValidationResult.Error(
                            "[red]Comment cannot be empty[/]")
                        : ValidationResult.Success())
        );

        var newComment = new Comment(
            body,
            userId,
            postId,
            parentCommentId
        );

        await commentRepository.AddAsync(newComment);
    }
}