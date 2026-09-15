using System.Globalization;
using Spectre.Console;

namespace CLI.UI.Helpers;

public class ConsoleHelper
{
    public static void PrintHeader(string title)
    {
        AnsiConsole.Write(new Rule($"[bold]{title}[/]").LeftJustified());
    }

    public static void PrintError(string message)
    {
        Console.WriteLine($"Error: {message}");
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            if (int.TryParse(Console.ReadLine(), out var value))
            {
                return value;
            }

            Console.WriteLine("Please enter a valid number.");
        }
    }

    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            var value = ReadInt(prompt);

            if (value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Please enter a number between {min} and {max}.");
        }
    }

    public static void PrintDivider()
    {
        Console.WriteLine("-----------------");
    }

    public static T? SelectFromList<T>(List<T> items, Func<T, string> display)
    {
        for (var i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {display(items[i])}");
        }

        Console.WriteLine("0. Go back");

        var option = ReadInt("Select option: ", 0, items.Count);

        return option == 0 ? default : items[option - 1];
    }
}