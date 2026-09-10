using CLI.UI.Helpers;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManageSubforums;

public class CreateSubforumView(ISubforumRepository subforumRepository)
{
    public async Task ShowAsync(int creatorId)
    {
        ConsoleHelper.PrintHeader("Create a subforum");
        
        Console.Write("Enter subforum name: ");
        var name = Console.ReadLine() ?? "";
        
        Console.Write("Enter a short description: ");
        var description = Console.ReadLine() ?? "";

        Subforum subforum = new(name, description, creatorId);

        await subforumRepository.AddAsync(subforum);
    }
}