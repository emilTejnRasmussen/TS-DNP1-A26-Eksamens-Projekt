using CLI.UI.Helpers;
using CLI.UI.ManagePosts;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManageSubforums;

public class ViewSubforumsView(ISubforumRepository subforumRepository, IPostRepository postRepository)
{
    private readonly CreatePostView _createPostView = new(postRepository);
    public async Task ShowAsync()
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Subforums");

            var subforum = SelectSubforum();

            if (subforum is null) return;

            await DisplaySubforumAsync(subforum);
        }
 
    }

    private async Task DisplaySubforumAsync(Subforum subforum)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader(subforum.Name);

            Console.WriteLine("1. View posts");
            Console.WriteLine("2. Create post");
            Console.WriteLine();
            Console.WriteLine("0. Go back");

            var option = ConsoleHelper.ReadInt("Select option: ", 0, 2);

            switch (option)
            {
                case 1:
                    Console.WriteLine("Not implemented");
                    break;

                case 2:
                    await _createPostView.ShowAsync();
                    break;

                case 0:
                    return;
            }
        }
    }

    private Subforum? SelectSubforum()
    {
        var subforums = subforumRepository.GetMany().ToList();

        for (var i = 0; i < subforums.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {subforums[i].Name}");
        }

        Console.WriteLine("0. Go back");

        var option = ConsoleHelper.ReadInt(
            "Select subforum: ",
            0,
            subforums.Count
        );

        return option == 0 ? null : subforums[option - 1];
    }
}