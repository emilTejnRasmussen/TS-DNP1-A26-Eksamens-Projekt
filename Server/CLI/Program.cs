using CLI.UI;
using InMemoryRepositories;
using RepositoryContract;

Console.WriteLine("Starting CLI app...");

IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
ISubforumRepository subforumRepository = new SubforumInMemoryRepository();

CliApp cliApp = new(userRepository, commentRepository, postRepository, subforumRepository);
await cliApp.StartAsync();