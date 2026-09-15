using CLI.UI.Helpers;
using CLI.UI.ManagePosts;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageSubforums;

public class ViewSubforumsView(
    ISubforumRepository subforumRepository, 
    IPostRepository postRepository, 
    ICommentRepository commentRepository,
    ICommentVoteRepository commentVoteRepository,
    IUserRepository userRepository)
{
    private readonly ViewPostsView _viewPostsView = new(
        postRepository, 
        commentRepository, 
        commentVoteRepository, 
        userRepository);

    public async Task ShowAsync(int userId)
    {
        while (true)
        {
            AnsiConsole.Clear();
            
            ConsoleHelper.PrintHeader("Subforums");
            
            var subforums = subforumRepository.GetMany().ToList();

            var creatorNames = new Dictionary<int, string>();
            var postCounts = new Dictionary<int, int>();

            foreach (var subforum in subforums)
            {
                var postCount = await postRepository.CountBySubforumIdAsync(subforum.Id);
                postCounts[subforum.Id] = postCount;

                if (creatorNames.ContainsKey(subforum.CreatorId)) continue;
                
                var user = await userRepository.GetSingleAsync(subforum.CreatorId);
                creatorNames[subforum.CreatorId] = user.Username;
            }

            var choices = subforums
                .Select(x => x.Id)
                .Append(0)
                .ToList();

            var selectedId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title($"[grey]  {"TITLE",-28}{"CREATED BY",-20}{"POSTS",5}[/]")
                    .UseConverter(id =>
                    {
                        if (id == 0) return "[grey]<- Go back[/]";

                        var subforum = subforums.First(x => x.Id == id);

                        return
                            $"{Markup.Escape(subforum.Name),-28}" +
                            $"{Markup.Escape(creatorNames[subforum.CreatorId]),-20}" +
                            $"[blue]{postCounts[subforum.Id],5}[/]";
                    })
                    .AddChoices(choices)
            );

            if (selectedId == 0)
                return;

            var selected = subforums.First(x => x.Id == selectedId);

            
            await _viewPostsView.ShowAsync(userId, selected);
        }
    }
}