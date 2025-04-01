using Spectre.Console;

namespace TrackerApp;

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
        TrackerUtils.LineMessage("Mood Update");
        var mood = _console.Prompt(new SelectionPrompt<string>()
            .Title("[bold green]What is your current mood?[/]")
            .AddChoices(trackedEmotions));

        TrackerUtils.ShowSelectedValue(mood);

        TrackerUtils.LineMessage("External Factors");
        // var triggerPresent = _console.Prompt(
        //     new TextPrompt<bool>("Do you want to record any triggers/external factors? ( If not, just hit enter )")
        //         .AddChoice(true)
        //         .AddChoice(false)
        //         .DefaultValue(false)
        //         .WithConverter(triggerPresent => triggerPresent ? "y" : "n"));

        var trigger =
            AnsiConsole.Prompt(
                new TextPrompt<string>("[[Optional]] Enter additional information, like triggers or external factors:")
                    .AllowEmpty());

        var triggerPresent = !string.IsNullOrEmpty(trigger);

        TrackerUtils.ShowSelectedValue(triggerPresent ? "Add more information" : "No additional information");

        // var trigger = "";
        // if (triggerPresent)
        // {
        //     trigger = _console.Prompt(new TextPrompt<string>("Provide more detail: "));
        //     TrackerUtils.ShowEnteredValue(trigger);
        // }

        var moodrecord = new MoodRecord(mood, trigger);
        var localtime = moodrecord.Timestamp.ToLocalTime().ToShortTimeString();

        if (triggerPresent)
            TrackerUtils.LineMessage(
                $"Saving update:  Time [yellow]{localtime}[/], Mood [yellow]{mood}[/] with a note: [yellow]{trigger}[/]");
        else
            TrackerUtils.LineMessage($"Saving update--  Time [yellow]{localtime}[/], Mood [yellow]{mood}[/]");

        TrackerUtils.EnterToContinue();

        return moodrecord;
    }

    public string GetReportChoice()
    {
        TrackerUtils.LineMessage("Report Options");

        return Markup.Remove(_console.Prompt(new SelectionPrompt<string>()
            .Title("Show breakdown/stats for the past [green]Day[/], [aqua]Week[/], or [red]Exit[/] to main menu?")
            .AddChoices("[green]Day[/]", "[aqua]Week[/]", "[red]Exit[/]")));
    }

    public string GetAdminOption()
    {
        return Markup.Remove(_console.Prompt(new SelectionPrompt<string>()
            .Title("[bold red]Select Admin Option:[/]")
            .AddChoices("Remove Last Mood Update", "Remove All Mood Updates", "Exit Admin Options")));
    }
}