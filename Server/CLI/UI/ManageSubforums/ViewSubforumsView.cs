using CLI.UI.Helpers;
using CLI.UI.ManagePosts;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManageSubforums;

public class ViewSubforumsView(ISubforumRepository subforumRepository, IPostRepository postRepository)
{
    private readonly CreatePostView _createPostView = new(postRepository);
    private Subforum? _currentSubforum;

    public async Task ShowAsync(int userId)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Subforums");

            _currentSubforum = SelectSubforum();

            if (_currentSubforum is null) return;

            await DisplaySubforumAsync(_currentSubforum, userId);
        }
 
    }

    private async Task DisplaySubforumAsync(Subforum subforum, int userId)
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
                    await _createPostView.ShowAsync(userId, subforum.Id);
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