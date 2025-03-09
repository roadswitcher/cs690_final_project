using Spectre.Console;
using System;
using System.Collections.Generic;

public static class UserInputHandler
{
    public static string GetUserInput(
        List<string> options)
    {
        var input = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Update your emotion from the following list[red], or select 'Report' or 'Exit'.[/]")
            .PageSize(20).MoreChoicesText("[grey]Use up/down arrows for more choices[/]")
            .AddChoices(options));

        return input;
        // return string.IsNullOrEmpty(input) ? defaultOption : input;
    }

    public static bool ProcessUserInput(string userInput)
    {
        Console.WriteLine($"User entered: {userInput}.");

        return !string.Equals(userInput, "Quit", StringComparison.OrdinalIgnoreCase);
    }
}