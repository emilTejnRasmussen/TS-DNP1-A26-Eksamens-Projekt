using CLI.UI.Helpers;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManageComments;

public class ViewCommentView(ICommentRepository commentRepository, ICommentVoteRepository commentVoteRepository)
{
    public async Task ShowAsync(int userId, Subforum subforum, Post post)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader(subforum.Name);
            Console.WriteLine(post.Title);

            ConsoleHelper.PrintDivider();

            var comments = commentRepository
                .GetMany()
                .Where(c => c.PostId == post.Id)
                .ToList();

            var comment = ConsoleHelper.SelectFromList(comments, c => c.Body);

            if (comment is null) return;

            await DisplayCommentAsync(comment, userId);
        }
    }

    private async Task DisplayCommentAsync(Comment comment, int userId)
    {
        while (true)
        {
            ConsoleHelper.PrintHeader(comment.Body);

            Console.WriteLine("1. Like comment");
            Console.WriteLine("2. Dislike comment");
            Console.WriteLine();
            Console.WriteLine("0. Go back");

            var option = ConsoleHelper.ReadInt("Select option: ", 0, 2);
            var commentVote = await commentVoteRepository.GetFromUserIdAndCommentIdAsync(userId, comment.Id);
            
            switch (option)
            {
                case 1:
                    if (commentVote is null)
                    {
                        CommentVote newCommentVote = new(userId, comment.Id, VoteType.Like);
                        await commentVoteRepository.AddAsync(newCommentVote);
                    }
                    else
                    {
                        commentVote.VoteType = VoteType.Like;
                        await commentVoteRepository.UpdateAsync(commentVote);
                    }
                    
                    break;

                case 2:
                    if (commentVote is null)
                    {
                        CommentVote newCommentVote = new(userId, comment.Id, VoteType.Dislike);
                        await commentVoteRepository.AddAsync(newCommentVote);
                    }
                    else
                    {
                        commentVote.VoteType = VoteType.Dislike;
                        await commentVoteRepository.UpdateAsync(commentVote);
                    }
                    break;

                case 0:
                    return;
            }
        }
    }
}