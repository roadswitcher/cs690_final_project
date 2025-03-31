using Spectre.Console;

namespace TrackerApp
{
    public class UserInputHandler(IAnsiConsole console)
    {
        private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));

        public string GetMainMenuChoice()
        {
            return Markup.Remove(_console.Prompt(new SelectionPrompt<string>()
                .Title("[green]Update[/] mood, [aqua]Report[/] data, or [red]Exit[/] app?")
                .AddChoices("[green]Update[/]", "[aqua]Report[/]", "[red]Admin Options[/]", "[red]Exit[/]")));
        }

        public MoodRecord GetMoodRecordUpdate(List<string> trackedEmotions)
        {
            _console.Write(new Rule("[cyan1]Mood Update[/]").LeftJustified().RuleStyle("cyan2"));
            string mood = _console.Prompt(new SelectionPrompt<string>()
                .Title("[bold green]What is your current mood?[/]")
                .AddChoices(trackedEmotions));

            _console.Write(new Rule("[cyan1]External Factors[/]").LeftJustified().RuleStyle("cyan2"));
            bool triggerPresent = _console.Prompt(new TextPrompt<bool>("Any triggers/factors to report?")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(triggerPresent => triggerPresent ? "y" : "n"));
            _console.WriteLine(triggerPresent ? "There was actually something, yes" : "Nope, nothing to report");

            string trigger = "";
            if (triggerPresent)
            {
                trigger = _console.Prompt(new TextPrompt<string>("Provide more detail: "));
            }

            return new MoodRecord(mood, trigger);
        }

        public void GetReportChoice()
        {
            _console.Write(new Rule("[cyan1]Report Options[/]").LeftJustified().RuleStyle("cyan2"));
            
        }
        public string GetAdminOption()
        {
            return Markup.Remove(_console.Prompt(new SelectionPrompt<string>()
                .Title("[bold red]Select Admin Option:[/]")
                .AddChoices("Remove Last Mood Update", "Remove All Mood Updates", "Exit Admin Options")));
        }
    }
}