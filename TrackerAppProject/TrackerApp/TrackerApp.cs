using Spectre.Console;
using System;
using System.Collections.Generic;

class TrackerApp
{
    private readonly IAnsiConsole _console;
    private readonly UserInputHandler _userInputHandler;

    public TrackerApp(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _userInputHandler = new UserInputHandler(_console);
    }

    public int Run(string[] args)
    {
        TrackerUtils.WelcomeScreen(args);

        RunMoodTracker();
        return 0;
    }

    private void RunMoodTracker()
    {
        bool stillRunning = true;

        while (stillRunning)
        {
            List<string> emotions = new() { "Happy", "Sad", "Mad", "Indifferent", "Quit" };

            string userInput = _userInputHandler.GetUserInput(emotions);

            stillRunning = UserInputHandler.ProcessUserInput(userInput);
        }
    }
}