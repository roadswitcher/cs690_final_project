using Spectre.Console;
using System;
using System.Collections.Generic;

class TrackerApp
{
    public static int Run(string[] args)
    {

        TrackerUtils.WelcomeScreen(args);

        RunMoodTracker();
        return 0;
    }

    private static void RunMoodTracker()
    {
        bool stillRunning = true;

        while (stillRunning)
        {
            List<string> emotions = new() { "Happy", "Sad", "Mad", "Indifferent" };
            string prompt = "Enter an emotion, or 'quit' to exit the app: ";
            string userInput = UserInputHandler.GetMoodUpdate(prompt, emotions, "Happy");

            stillRunning = UserInputHandler.ProcessUserInput(userInput);
        }

        // TODO: Add shutdown logic
        // TODO: Stretch goal:  capture exception, ensure orderly shutdown?

    }
}

