using Spectre.Console;
using System;


public static class TrackerLauncher
{
    public static int Main(string[] args)
    {
        var app = new TrackerApp();

        return app.Run(args);
    }
}

class TrackerApp
{
    public int Run(string[] args)
    {
        AnsiConsole.Markup("[bold yellow]Hello World![/]");
        AnsiConsole.WriteLine();

        var name = AnsiConsole.Ask<string>("What's your [green]name[/]?");
        AnsiConsole.MarkupLine($"[blue]Nice to meet you, {name}[/]");

        return 0;
    }
}

// string GetUserInput(string prompt, List<string> options, string defaultOption)
// {
//     if (!options.Contains(defaultOption))
//     {
//         throw new ArgumentException("ERROR: Default option not provided.");
//     }

//     Console.Write(prompt);
//     Console.Write($"[{defaultOption}]");

//     string input = Console.ReadLine() ?? String.Empty;

//     return (string.IsNullOrEmpty(input)) ? defaultOption : input;
// }


// bool ProcessUserInput(string userinput)
// {
//     Console.WriteLine($"User entered: {userinput}.");

//     if (String.Equals(userinput, "quit", StringComparison.OrdinalIgnoreCase))
//     {
//         return false;
//     }
//     else
//     {
//         return true;
//     }
// }
// bool still_running = true;

// while (still_running)
// {

//     List<string> emotions = new List<string> { "Happy", "Sad", "Mad", "Indifferent" };
//     string prompt = "Enter an emotion, or 'quit' to exit the app: ";
//     string userinput = GetUserInput(prompt, emotions, "Happy");

//     still_running = ProcessUserInput(userinput);
// }
