using CLI.UI.Helpers;
using Entities;
using RepositoryContract;
using Spectre.Console;

namespace CLI.UI.ManageUsers;

public class AuthUser(IUserRepository userRepository)
{
    private readonly CreateUserView _createUserView = new(userRepository);

    public async Task<User> ShowAsync()
    {
        User user = null;

        while (user is null)
        {
            AnsiConsole.Clear();
            
            ConsoleHelper.PrintHeader("Auth");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an [green]option[/]:")
                    .AddChoices("Login", "Create new account"));

            switch (choice)
            {
                case "Login":
                    Console.WriteLine("Not implemented bc in memory");
                    break;
                case "Create new account":
                    user = await _createUserView.ShowAsync();
                    break;
            }
        }
        
        return user;
    }
}