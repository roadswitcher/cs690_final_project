using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace TrackerApp
{
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

    class Tracker
    {
        private readonly IAnsiConsole _console;
        private readonly UserInputHandler _userInputHandler;

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

        private void RunMoodTracker()
        {
            bool shouldAppDie = false;


            while (!shouldAppDie)
            {
                List<string> trackedEmotions = new() { "Happy", "Sad", "Mad", "Wistful", "Indifferent" };

                var userInput = _userInputHandler.GetUserInput(trackedEmotions);

                if (userInput is string choice)
                {
                    Console.WriteLine($"User picked: {choice}");
                    shouldAppDie = string.Equals(choice, "Exit", StringComparison.OrdinalIgnoreCase);
                }
                else if (userInput is MoodRecord mood)
                {
                    Console.WriteLine($"Mood: {mood.Mood}");
                    Console.WriteLine($"Trigger: {mood.Trigger}");
                }
                
            }
        }
    }
}