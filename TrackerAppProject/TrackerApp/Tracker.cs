using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace TrackerApp
{
    class Tracker
    {
        private readonly IAnsiConsole _console;
        private readonly UserInputHandler _userInputHandler;

        public class MoodRecord
        {
            public string Mood { get; }
            public string? Trigger { get; }

            public MoodRecord(string mood, string? trigger = null)
            {
                Mood = mood;
                Trigger = trigger;
            }
        }

        public Tracker(IAnsiConsole console)
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

        public void DebugPrint(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#else
        // Nothing happens in Release mode
#endif
        }

        private void RunMoodTracker()
        {
            bool stillRunning = true;


            while (stillRunning)
            {
                List<string> trackedEmotions = new() { "Happy", "Sad", "Mad", "Indifferent" };

                var userInput = _userInputHandler.GetUserInput(trackedEmotions);

                if (userInput is string choice)
                {
                    DebugPrint($"User picked: {choice}");
                    stillRunning = !string.Equals(choice, "Exit", StringComparison.OrdinalIgnoreCase);
                }
                else if (userInput is MoodRecord mood)
                {
                    // TODO: Update/store user mood
                }

                // stillRunning = UserInputHandler.ProcessUserInput(userInput);
            }
        }
    }
}