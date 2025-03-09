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

            AnsiConsole.Write(new Rule("[cyan1]Mood Update[/]").LeftJustified().RuleStyle("cyan2"));
            string mood = _console.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]What is your current mood?[/]")
                    .AddChoices(trackedEmotions));

            AnsiConsole.Write(new Rule("[cyan1]External Factors[/]").LeftJustified().RuleStyle("cyan2"));
            bool triggerPresent = _console.Prompt(
                new TextPrompt<bool>("Any triggers/factors to report?")
                .AddChoice(true).AddChoice(false).DefaultValue(false).WithConverter(triggerPresent => triggerPresent ? "y" : "n")
                );
            Console.WriteLine(triggerPresent ? "There was actually something, yes" : "Nope, nothing to report");

            var trigger = "";
            if (triggerPresent)
            {
                trigger = _console.Prompt(
                    new TextPrompt<string>("Provide more detail: ")
                    );
            }

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