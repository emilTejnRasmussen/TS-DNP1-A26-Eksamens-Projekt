using CLI.UI.Helpers;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageSubforums;

public class CreateSubforumView(ISubforumRepository subforumRepository)
{
    public async Task ShowAsync(int creatorId)
    {
        var name = AnsiConsole.Ask<string>("What's your subforums [green]name[/]?");
        AnsiConsole.MarkupLine($"Subforum, [blue]{name} created[/]!");
        var description = AnsiConsole.Ask<string>("Enter a short [green]description[/]:");
        

        Subforum subforum = new(name, description, creatorId);

        await subforumRepository.AddAsync(subforum);
    }
}