using CLI.UI.ManageComments;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManagePosts;

public class ViewPostsView(
    IPostRepository postRepository,
    ICommentRepository commentRepository,
    ICommentVoteRepository commentVoteRepository,
    IUserRepository userRepository)
{
    private readonly CreatePostView _createPostView = new(postRepository);
    private readonly CreateCommentView _createCommentView = new(commentRepository);
    private readonly ViewCommentView _viewCommentView =
        new(commentRepository, commentVoteRepository);

    public async Task ShowAsync(int userId, Subforum subforum)
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine($"[bold]{Markup.Escape(subforum.Name)}[/]");
            AnsiConsole.MarkupLine($"[grey]{Markup.Escape(subforum.Description)}[/]");
            AnsiConsole.WriteLine();

            var posts = postRepository
                .GetMany()
                .Where(post => post.SubforumId == subforum.Id)
                .ToList();

            var creatorNames = new Dictionary<int, string>();

            foreach (var post in posts.Where(post => !creatorNames.ContainsKey(post.UserId)))
            {
                var user = await userRepository.GetSingleAsync(post.UserId);
                creatorNames[post.UserId] = user.Username;
            }

            var choices = posts
                .Select(post => post.Id)
                .Append(-1) 
                .Append(0)  
                .ToList();

            var selectedId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title(
                        $"[grey]  {"TITLE",-32}{"WRITTEN BY",-20}[/]")
                    .PageSize(10)
                    .UseConverter(id =>
                    {
                        switch (id)
                        {
                            case -1:
                                return "[green]+ Create post[/]";
                            case 0:
                                return "[grey]<- Go back[/]";
                            default:
                            {
                                var post = posts.First(post => post.Id == id);

                                return
                                    $"{Markup.Escape(post.Title),-32}" +
                                    $"{Markup.Escape(creatorNames[post.UserId]),-20}";
                            }
                        }
                    })
                    .AddChoices(choices)
            );

            switch (selectedId)
            {
                case 0:
                    return;

                case -1:
                    await _createPostView.ShowAsync(userId, subforum.Id);
                    continue;
            }

            var selectedPost = posts.First(post => post.Id == selectedId);

            await DisplayPostAsync(subforum, selectedPost, userId);
        }
    }

    private async Task DisplayPostAsync(
        Subforum subforum,
        Post post,
        int userId)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[bold]{Markup.Escape(post.Title)}[/]")
                    .AddChoices(
                        "View comments",
                        "Create comment",
                        "<- Go back"
                    )
            );

            switch (option)
            {
                case "View comments":
                    await _viewCommentView.ShowAsync(userId, subforum, post);
                    break;

                case "Create comment":
                    await _createCommentView.ShowAsync(userId, post.Id);
                    break;

                case "<- Go back":
                    return;
            }
        }
    }
}