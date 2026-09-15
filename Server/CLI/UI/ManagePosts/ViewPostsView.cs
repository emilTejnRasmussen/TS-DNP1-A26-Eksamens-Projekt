using CLI.UI.ManageComments;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManagePosts;

public class ViewPostsView(
    IPostRepository postRepository,
    IPostVoteRepository postVoteRepository,
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
            var commentCounts = new Dictionary<int, int>();
            var likeCounts = new Dictionary<int, int>();
            var dislikeCounts = new Dictionary<int, int>();

            foreach (var post in posts)
            {
                if (!creatorNames.ContainsKey(post.UserId))
                {
                    var user = await userRepository.GetSingleAsync(post.UserId);
                    creatorNames[post.UserId] = user.Username;
                }

                commentCounts[post.Id] =
                    await commentRepository.CountByPostIdAsync(post.Id);

                likeCounts[post.Id] =
                    await postVoteRepository.CountByPostIdAndVoteTypeAsync(
                        post.Id,
                        VoteType.Like);

                dislikeCounts[post.Id] =
                    await postVoteRepository.CountByPostIdAndVoteTypeAsync(
                        post.Id,
                        VoteType.Dislike);
            }

            var choices = posts
                .Select(post => post.Id)
                .Append(-1)
                .Append(0)
                .ToList();

            var selectedId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title(
                        $"[grey]  " +
                        $"{"TITLE",-28}" +
                        $"{"WRITTEN BY",-16}" +
                        $"{"COMMENTS",10}" +
                        $"{"LIKES",7}" +
                        $"{"DISLIKES",10}[/]")
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
                                var post = posts.First(post => post.Id == id);

                                return
                                    $"{Markup.Escape(post.Title),-28}" +
                                    $"{Markup.Escape(creatorNames[post.UserId]),-16}" +
                                    $"[blue]{commentCounts[post.Id],10}[/]" +
                                    $"[green]{likeCounts[post.Id],7}[/]" +
                                    $"[red]{dislikeCounts[post.Id],10}[/]";
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

            await DisplayPostAsync(selectedPost, userId);
        }
    }

    private async Task DisplayPostAsync(Post post, int userId)
    {
        const int createCommentAction = -1;
        const int likeAction = -2;
        const int dislikeAction = -3;
        const int goBackAction = 0;

        while (true)
        {
            AnsiConsole.Clear();

            var author = await userRepository.GetSingleAsync(post.UserId);

            var commentCount =
                await commentRepository.CountByPostIdAsync(post.Id);

            var likeCount =
                await postVoteRepository.CountByPostIdAndVoteTypeAsync(
                    post.Id,
                    VoteType.Like);

            var dislikeCount =
                await postVoteRepository.CountByPostIdAndVoteTypeAsync(
                    post.Id,
                    VoteType.Dislike);

            AnsiConsole.Write(
                new Panel(Markup.Escape(post.Body))
                    .Header($"[bold]{Markup.Escape(post.Title)}[/]")
                    .Expand()
            );

            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine(
                $"[grey]Written by[/] {Markup.Escape(author.Username)}   " +
                $"[blue]{commentCount} comments[/]   " +
                $"[green]{likeCount} likes[/]   " +
                $"[red]{dislikeCount} dislikes[/]"
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
                replyCounts[comment.Id] =
                    await commentRepository.CountByParentCommentIdAsync(comment.Id);
            }

            var choices = comments
                .Select(comment => comment.Id)
                .Append(createCommentAction)
                .Append(likeAction)
                .Append(dislikeAction)
                .Append(goBackAction)
                .ToList();

            var selectedId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title($"[grey]  {"COMMENT",-50}{"REPLIES",7}[/]")
                    .PageSize(15)
                    .UseConverter(id =>
                    {
                        switch (id)
                        {
                            case createCommentAction:
                                return "[green]+ Create comment[/]";

                            case likeAction:
                                return "[green]Like post[/]";

                            case dislikeAction:
                                return "[red]Dislike post[/]";

                            case goBackAction:
                                return "[grey]<- Go back[/]";

                            default:
                                var comment =
                                    comments.First(c => c.Id == id);

                                return
                                    $"{Markup.Escape(comment.Body),-50}" +
                                    $"[blue]{replyCounts[comment.Id],7}[/]";
                        }
                    })
                    .AddChoices(choices)
            );

            switch (selectedId)
            {
                case createCommentAction:
                    await _createCommentView.ShowAsync(
                        userId,
                        post.Id);
                    break;

                case likeAction:
                    await VotePostAsync(
                        post.Id,
                        userId,
                        VoteType.Like);
                    break;

                case dislikeAction:
                    await VotePostAsync(
                        post.Id,
                        userId,
                        VoteType.Dislike);
                    break;

                case goBackAction:
                    return;

                default:
                    var selectedComment =
                        comments.First(c => c.Id == selectedId);

                    await _viewCommentView.ShowAsync(
                        userId,
                        selectedComment);
                    break;
            }
        }
    }
    
    private async Task VotePostAsync(
        int postId,
        int userId,
        VoteType voteType)
    {
        var vote =
            await postVoteRepository.GetFromUserIdAndPostIdAsync(userId, postId);

        if (vote is null)
        {
            var newVote = new PostVote(
                userId,
                postId,
                voteType);

            await postVoteRepository.AddAsync(newVote);
        }
        else
        {
            vote.VoteType = voteType;

            await postVoteRepository.UpdateAsync(vote);
        }
    }
}