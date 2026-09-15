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

            AnsiConsole.Write(
                new Panel(Markup.Escape(post.Body))
                    .Header($"[bold]{Markup.Escape(post.Title)}[/]")
                    .Expand()
            );

            AnsiConsole.WriteLine();

            var comments = commentRepository
                .GetMany()
                .Where(comment =>
                    comment.PostId == post.Id &&
                    comment.ParentCommentId == null)
                .ToList();
            
            var replyCounts = new Dictionary<int, int>();

            foreach (var comment in comments)
            {
                replyCounts[comment.Id] = await commentRepository.CountByParentCommentIdAsync(comment.Id);
            }

            var choices = comments
                .Select(comment => comment.Id)
                .Append(-1) // Create comment
                .Append(0)  // Go back
                .ToList();

            var selectedId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title($"[grey]  {"COMMENT",-50}{"REPLIES",7}[/]")
                    .PageSize(10)
                    .UseConverter(id =>
                    {
                        switch (id)
                        {
                            case -1:
                                return "[green]+ Create comment[/]";
                            case 0:
                                return "[grey]<- Go back[/]";
                            default:
                            {
                                var comment = comments.First(c => c.Id == id);

                                return
                                    $"{Markup.Escape(comment.Body),-50}" +
                                    $"[blue]{replyCounts[comment.Id],7}[/]";
                            }
                        }
                    })
                    .AddChoices(choices)
            );

            switch (selectedId)
            {
                case -1:
                    await _createCommentView.ShowAsync(userId, post.Id);
                    break;

                case 0:
                    return;

                default:
                    var selectedComment = comments.First(c => c.Id == selectedId);
                    await _viewCommentView.ShowAsync(userId, selectedComment);
                    break;
            }
        }
    }
}