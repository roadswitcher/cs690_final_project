using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TrackerApp
{
    public class UserInputHandler(IAnsiConsole console)
    {
        private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));

        private MoodRecord GetMoodRecordUpdate(List<string> trackedEmotions)
        {

            AnsiConsole.Write(new Rule("[cyan1]Mood Update[/]").LeftJustified().RuleStyle("cyan2"));
            var mood = _console.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]What is your current mood?[/]")
                    .AddChoices(trackedEmotions));

            AnsiConsole.Write(new Rule("[cyan1]External Factors[/]").LeftJustified().RuleStyle("cyan2"));
            var triggerPresent = _console.Prompt(
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

            switch (choice)
            {
                case "Exit":
                    return choice;
                case "Report":
                    return choice;
                default:
                    Console.WriteLine("Updating!");
                    return GetMoodRecordUpdate(trackedEmotions);
            }
        }

    }
}