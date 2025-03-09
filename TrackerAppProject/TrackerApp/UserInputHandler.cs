using System;
using System.Collections.Generic;
using Spectre.Console;

public static class UserInputHandler
{
    public static string GetUserInput(
        List<string> options, string defaultOption)
    {
        if (!options.Contains(defaultOption))
        {
            throw new ArgumentException("ERROR: Default option not provided.");
        }
        string prompt = "Enter an emotion, or 'quit' to exit the app: ";
        Console.Write($"{prompt}");
        Console.Write($"[{defaultOption}] ");

        string input = Console.ReadLine() ?? string.Empty;
        return string.IsNullOrEmpty(input) ? defaultOption : input;
    }

    public static bool ProcessUserInput(string userInput)
    {
        Console.WriteLine($"User entered: {userInput}.");

        return !string.Equals(userInput, "quit", StringComparison.OrdinalIgnoreCase);
    }
}
