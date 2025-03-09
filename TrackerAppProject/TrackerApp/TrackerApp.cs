using Spectre.Console;
using System;
using System.Collections.Generic;

class TrackerApp
{
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

            string userInput = UserInputHandler.GetUserInput(emotions);

            stillRunning = UserInputHandler.ProcessUserInput(userInput);
        }

    }
}