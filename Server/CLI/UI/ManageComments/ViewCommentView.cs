using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageComments;

public class ViewCommentView(
    ICommentRepository commentRepository,
    ICommentVoteRepository commentVoteRepository)
{
    private readonly CreateCommentView _createCommentView = new(commentRepository);

    public async Task ShowAsync(int userId, Comment comment)
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new Panel(Markup.Escape(comment.Body))
                    .Header("[bold]Comment[/]")
                    .Expand()
            );

            AnsiConsole.WriteLine();

            var replies = commentRepository
                .GetMany()
                .Where(c => c.ParentCommentId == comment.Id)
                .ToList();

            const int replyAction = -1;
            const int likeAction = -2;
            const int dislikeAction = -3;
            const int goBackAction = 0;

            var choices = replies
                .Select(reply => reply.Id)
                .Append(replyAction)
                .Append(likeAction)
                .Append(dislikeAction)
                .Append(goBackAction)
                .ToList();

            var selectedId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[grey]Replies[/]")
                    .PageSize(10)
                    .UseConverter(id =>
                    {
                        return id switch
                        {
                            replyAction => "[green]+ Reply[/]",
                            likeAction => "Like",
                            dislikeAction => "Dislike",
                            goBackAction => "[grey]<- Go back[/]",
                            _ => Markup.Escape(
                                replies.First(reply => reply.Id == id).Body)
                        };
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