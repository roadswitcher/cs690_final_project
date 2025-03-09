using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrackerApp
{
    public class UserInputHandler
    {
        private readonly IAnsiConsole _console;

        public UserInputHandler(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public MoodRecord GetMoodRecordUpdate(List<string> trackedEmotions)
        {
            string mood = _console.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]What is your current mood?[/]")
                    .AddChoices(trackedEmotions));

            string trigger = "foo";

            return new MoodRecord(mood, trigger);
        }

        public object GetUserInput(List<string> trackedEmotions)
        {
            var choice = Markup.Remove(_console.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Update[/] mood, [aqua]Report[/] data, or [red]Exit[/] app?")
                    .AddChoices("[green]Update[/]", "[aqua]Report[/]", "[red]Exit[/]")
            ));

            if (choice == "Exit")
            {
                return choice;
            }
            else if (choice == "Report")
            {
                return choice;
            }
            else
            {
                Console.WriteLine("Updating!");
                return GetMoodRecordUpdate(trackedEmotions);
            }
        }

        public static bool ProcessUserInput(string userInput)
        {
            Console.WriteLine($"User entered: {userInput}.");
            return !string.Equals(userInput, "Quit", StringComparison.OrdinalIgnoreCase);
        }
    }
}