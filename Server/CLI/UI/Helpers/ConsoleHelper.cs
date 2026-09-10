namespace CLI.UI.Helpers;

public class ConsoleHelper
{
    public static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ===");
        Console.WriteLine();
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
}