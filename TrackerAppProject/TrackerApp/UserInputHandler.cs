using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public class UserInputHandler
{
    private readonly IAnsiConsole _console;

    public UserInputHandler(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    public object GetUserInput(List<string> options)
    {
        // var choice = = _console.Prompt(
        //     new SelectionPrompt<string>()
        //         .Title("Update your emotion from the following list[red], or select 'Report' or 'Exit'.[/]")
        //         .PageSize(20)
        //         .MoreChoicesText("[grey]Use up/down arrows for more choices[/]")
        //         .AddChoices(options)
        // );
        var choice = _console.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Update[/] mood, [aqua]Report[/] data, or [red]Exit[/] app?")
                .AddChoices("[green]Update[/]", "[aqua]Report[/]", "[red]Exit[/]")
        );

        // switch (choice)
        // {
        //     case "Exit", "Report":
        //         return choice;

        //     case "Update":
        //         var response = GetMoodUpdate
        // }

        return Markup.Remove(choice);
    }

    public static bool ProcessUserInput(string userInput)
    {
        Console.WriteLine($"User entered: {userInput}.");
        return !string.Equals(userInput, "Quit", StringComparison.OrdinalIgnoreCase);
    }
}