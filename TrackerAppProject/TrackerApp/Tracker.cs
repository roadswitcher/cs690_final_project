using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace TrackerApp
{
    internal class Tracker
    {
        private readonly UserInputHandler _userInputHandler;
        private readonly DataStore _dataStore;

        public Tracker(IAnsiConsole console)
        {
            IAnsiConsole console1 = console ?? throw new ArgumentNullException(nameof(console));
            _userInputHandler = new UserInputHandler(console1);
            _dataStore = DataStore.Instance;
        }

        public int Run(string[] args)
        {
            TrackerUtils.WelcomeScreen(args);

            RunMoodTracker();
            return 0;
        }

        private void RunMoodTracker()
        {
            bool shouldAppDie = false;

            while (!shouldAppDie)
            {
                List<string> trackedEmotions = ["Happy", "Sad", "Mad", "Wistful", "Indifferent"];

                object userInput = _userInputHandler.GetUserInput(trackedEmotions);

                switch (userInput)
                {
                    case string choice:
                        Console.WriteLine($"User picked: {choice}");
                        shouldAppDie = string.Equals(choice, "Exit", StringComparison.OrdinalIgnoreCase);
                        break;
                    case MoodRecord mood:
                        Console.WriteLine($"Mood: {mood.Mood}");
                        Console.WriteLine($"Trigger: {mood.Trigger}");
                        _dataStore.AddMoodRecord(mood);
                        break;
                }
            }
        }
    }
}