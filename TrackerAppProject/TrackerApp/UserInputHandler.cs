using Spectre.Console;
using System;
using System.Collections.Generic;

public class UserInputHandler
{
    private readonly IAnsiConsole _console;

    public UserInputHandler(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    public string GetUserMood(List<string> moods)
    {
        return _console.Prompt(
    new SelectionPrompt<string>()
        .Title("Update your emotion from the following list[red], or select 'Report' or 'Exit'.[/]")
        .PageSize(20)
        .MoreChoicesText("[grey]Use up/down arrows for more choices[/]")
        .AddChoices(moods)
        );
    }
    public string GetUserInput(List<string> options)
    {
        return _console.Prompt(
            new SelectionPrompt<string>()
                .Title("Update Mood, View Data, or Quit?")
                .AddChoices("Update Mood", "View Data", "Quit"));

    }

    public static bool ProcessUserInput(string userInput)
    {
        Console.WriteLine($"User entered: {userInput}.");
        return !string.Equals(userInput, "Quit", StringComparison.OrdinalIgnoreCase);
    }
}