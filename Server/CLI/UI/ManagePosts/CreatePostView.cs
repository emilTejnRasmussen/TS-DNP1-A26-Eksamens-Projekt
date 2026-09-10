using CLI.UI.Helpers;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IPostRepository postRepository)
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task ShowAsync(int userId, int subforumId)
    {
        ConsoleHelper.PrintHeader("Create Post");

        Console.Write("Enter title: ");
        var title = Console.ReadLine() ?? "";
        
        Console.Write("Enter body: ");
        var body = Console.ReadLine() ?? "";
        
        
        Post post = new(title, body, userId, subforumId);
        var createdPost = await _postRepository.AddAsync(post);

        Console.WriteLine($"Created post: {createdPost}");
    }
}