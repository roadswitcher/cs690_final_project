using Spectre.Console;
using System;
using System.Collections.Generic;

class TrackerApp
{
  public int Run(string[] args)
  {


    AnsiConsole.Clear();

    // TODO: Look up how to pass debug flags in, wrap these messages 
    int consoleWidth = AnsiConsole.Profile.Width;
    AnsiConsole.MarkupLine($"[bold yellow]Current Console Width: {consoleWidth}[/]");
    AnsiConsole.MarkupLine($"[bold yellow]Args Length: {args.Length}[/]");


    AnsiConsole.Write(
        new FigletText("Welcome!")
            .Centered()
            .Color(Color.Cyan1)
    );

    AnsiConsole.Write(
        new Rule("[bold green]Let's Get Started[/]")
            .Centered()
            .RuleStyle("green")
    );

    RunMoodTracker();
    return 0;
  }

  private void RunMoodTracker()
  {
    bool stillRunning = true;

    while (stillRunning)
    {
      List<string> emotions = new() { "Happy", "Sad", "Mad", "Indifferent" };
      string prompt = "Enter an emotion, or 'quit' to exit the app: ";
      string userInput = UserInputHandler.GetUserInput(prompt, emotions, "Happy");

      stillRunning = UserInputHandler.ProcessUserInput(userInput);
    }

    // TODO: Add shutdown logic
    // TODO: Stretch goal:  capture exception, ensure orderly shutdown?

  }
}
