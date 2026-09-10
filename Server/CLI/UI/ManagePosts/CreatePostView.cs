using CLI.UI.Helpers;
using RepositoryContract;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IPostRepository postRepository)
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task ShowAsync()
    {
        ConsoleHelper.PrintHeader("Create Post");
        
        
    }
}