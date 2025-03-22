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
        private DataStore _dataStore;
        private bool _isLoggedIn;
        

        public Tracker(IAnsiConsole console)
        {
            IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));
            _userInputHandler = new UserInputHandler(_console);
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

            // TODO: Login and Datastore
            // -- if there's no datastore, clean login
            // -- existing datastore?   who logged in, ask password again


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
                        break;
                }
            }
        }
    }
}