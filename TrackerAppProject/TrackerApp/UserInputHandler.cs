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
        TrackerUtils.LineMessage("Mood Update: [green]What is your current mood?[/]");
        var mood = _console.Prompt(new SelectionPrompt<string>()
            .AddChoices(trackedEmotions));

        TrackerUtils.ShowSelectedValue(mood);

        TrackerUtils.LineMessage(
            "External Factors: Do you want to record more information, like triggers or external factors?");

        var trigger =
            AnsiConsole.Prompt(
                new TextPrompt<string>("[green][[Optional]] (hit Enter):[/]")
                    .AllowEmpty());

        var triggerPresent = !string.IsNullOrEmpty(trigger);

        var moodRecord = new MoodRecord(mood, trigger);
        var localtime = moodRecord.Timestamp.ToLocalTime().ToShortTimeString();

        TrackerUtils.LineMessage(
            triggerPresent
                ? $"Saving update:  Time [yellow]{localtime}[/], Mood [yellow]{mood}[/], with additional note: [yellow]{trigger}[/]"
                : $"Saving update:  Time [yellow]{localtime}[/], Mood [yellow]{mood}[/], no additional factors");

        TrackerUtils.EnterToContinue();

        return moodRecord;
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
        TrackerUtils.LineMessage("[red]Admin Options:[/]", "red3");

        return Markup.Remove(_console.Prompt(new SelectionPrompt<string>()
            .Title("[bold red]Select Admin Option:[/]")
            .AddChoices("Remove Last Update", "Exit to Main Menu")));
    }
}