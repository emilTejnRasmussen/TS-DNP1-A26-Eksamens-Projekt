using CLI.UI.Helpers;
using Entities;
using RepositoryContract;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository)
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<User> ShowAsync()
    {
        ConsoleHelper.PrintHeader("Create User");
        
        Console.Write("Enter username: ");
        var username = Console.ReadLine() ?? "";
        
        Console.Write("Enter password: ");
        var password = Console.ReadLine() ?? "";

        User user = new(username, password);

        var created = await userRepository.AddAsync(user);
        
        Console.WriteLine($"Created User with ID {created.Id}");

        return created;
    }
}