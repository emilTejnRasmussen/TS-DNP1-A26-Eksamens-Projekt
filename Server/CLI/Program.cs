using CLI.UI;
using FileRepositories;
using InMemoryRepositories;
using RepositoryContract;

Console.WriteLine("Starting CLI app...");

IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();
ISubforumRepository subforumRepository = new SubforumFileRepository();
ICommentVoteRepository commentVoteRepository = new CommentVoteFileRepository();
IPostVoteRepository postVoteRepository = new PostVoteFileRepository();

CliApp cliApp = new(userRepository, commentRepository, postRepository, subforumRepository, commentVoteRepository, postVoteRepository);
await cliApp.StartAsync();