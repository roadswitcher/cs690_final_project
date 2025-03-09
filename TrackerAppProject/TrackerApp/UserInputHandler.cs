using Spectre.Console;
using System;
using System.Collections.Generic;

public class UserInputHandler
{
    private readonly IAnsiConsole _console;

    // Dependency injection for IAnsiConsole allows easier testing
    public UserInputHandler(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    public string GetUserInput(List<string> options)
    {
        return _console.Prompt(
            new SelectionPrompt<string>()
                .Title("Update your emotion from the following list[red], or select 'Report' or 'Exit'.[/]")
                .PageSize(20)
                .MoreChoicesText("[grey]Use up/down arrows for more choices[/]")
                .AddChoices(options)
        );
    }

    public static bool ProcessUserInput(string userInput)
    {
        Console.WriteLine($"User entered: {userInput}.");
        return !string.Equals(userInput, "Quit", StringComparison.OrdinalIgnoreCase);
    }
}