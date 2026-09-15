using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageComments;

public class ViewCommentView(
    ICommentRepository commentRepository,
    ICommentVoteRepository commentVoteRepository,
    IUserRepository userRepository)
{
    private readonly CreateCommentView _createCommentView =
        new(commentRepository);

    public async Task ShowAsync(int userId, Comment comment)
    {
        const int replyAction = -1;
        const int likeAction = -2;
        const int dislikeAction = -3;
        const int goBackAction = 0;

        while (true)
        {
            AnsiConsole.Clear();

            var author = await userRepository.GetSingleAsync(comment.UserId);
            var replyCount = await commentRepository.CountByParentCommentIdAsync(comment.Id);
            var likeCount = await commentVoteRepository.CountByCommentIdAndVoteTypeAsync(comment.Id, VoteType.Like);
            var dislikeCount = await commentVoteRepository.CountByCommentIdAndVoteTypeAsync(comment.Id, VoteType.Dislike);

            AnsiConsole.Write(
                new Panel(Markup.Escape(comment.Body))
                    .Header("[bold]Comment[/]")
                    .Expand()
            );

            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine(
                $"[grey]Written by[/] {Markup.Escape(author.Username)}   " +
                $"[blue]{replyCount} replies[/]   " +
                $"[green]{likeCount} likes[/]   " +
                $"[red]{dislikeCount} dislikes[/]"
            );

            AnsiConsole.WriteLine();

            var replies = commentRepository
                .GetMany()
                .Where(c => c.ParentCommentId == comment.Id)
                .ToList();

            var creatorNames = new Dictionary<int, string>();
            var replyCounts = new Dictionary<int, int>();
            var likeCounts = new Dictionary<int, int>();
            var dislikeCounts = new Dictionary<int, int>();

            foreach (var reply in replies)
            {
                if (!creatorNames.ContainsKey(reply.UserId))
                {
                    var replyAuthor =
                        await userRepository.GetSingleAsync(reply.UserId);

                    creatorNames[reply.UserId] =
                        replyAuthor.Username;
                }

                replyCounts[reply.Id] =
                    await commentRepository
                        .CountByParentCommentIdAsync(reply.Id);

                likeCounts[reply.Id] =
                    await commentVoteRepository
                        .CountByCommentIdAndVoteTypeAsync(
                            reply.Id,
                            VoteType.Like);

                dislikeCounts[reply.Id] =
                    await commentVoteRepository
                        .CountByCommentIdAndVoteTypeAsync(
                            reply.Id,
                            VoteType.Dislike);
            }

            var choices = replies
                .Select(reply => reply.Id)
                .Append(replyAction)
                .Append(likeAction)
                .Append(dislikeAction)
                .Append(goBackAction)
                .ToList();

            var selectedId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title(
                        $"[grey]  " +
                        $"{"REPLY",-32}" +
                        $"{"WRITTEN BY",-16}" +
                        $"{"REPLIES",9}" +
                        $"{"LIKES",7}" +
                        $"{"DISLIKES",10}[/]")
                    .PageSize(15)
                    .UseConverter(id =>
                    {
                        switch (id)
                        {
                            case replyAction:
                                return "[green]+ Reply[/]";

                            case likeAction:
                                return "[green]Like comment[/]";

                            case dislikeAction:
                                return "[red]Dislike comment[/]";

                            case goBackAction:
                                return "[grey]<- Go back[/]";

                            default:
                                var reply =
                                    replies.First(r => r.Id == id);

                                return
                                    $"{Markup.Escape(reply.Body),-32}" +
                                    $"{Markup.Escape(creatorNames[reply.UserId]),-16}" +
                                    $"[blue]{replyCounts[reply.Id],9}[/]" +
                                    $"[green]{likeCounts[reply.Id],7}[/]" +
                                    $"[red]{dislikeCounts[reply.Id],10}[/]";
                        }
                    })
                    .AddChoices(choices)
            );

            switch (selectedId)
            {
                case replyAction:
                    await _createCommentView.ShowAsync(
                        userId,
                        comment.PostId,
                        comment.Id);
                    break;

                case likeAction:
                    await VoteAsync(
                        comment,
                        userId,
                        VoteType.Like);
                    break;

                case dislikeAction:
                    await VoteAsync(
                        comment,
                        userId,
                        VoteType.Dislike);
                    break;

                case goBackAction:
                    return;

                default:
                    var selectedReply =
                        await commentRepository.GetSingleAsync(selectedId);

                    await ShowAsync(userId, selectedReply);
                    break;
            }
        }
    }

    private async Task VoteAsync(
        Comment comment,
        int userId,
        VoteType voteType)
    {
        var vote =
            await commentVoteRepository
                .GetFromUserIdAndCommentIdAsync(
                    userId,
                    comment.Id);

        if (vote is null)
        {
            var newVote = new CommentVote(
                userId,
                comment.Id,
                voteType);

            await commentVoteRepository.AddAsync(newVote);
        }
        else
        {
            vote.VoteType = voteType;

            await commentVoteRepository.UpdateAsync(vote);
        }
    }
}