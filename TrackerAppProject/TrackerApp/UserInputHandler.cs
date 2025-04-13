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
            _console.Prompt(
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
            .Title(
                "Show report for [green]Today[/], [cyan]Pick a day[/], [aqua]Past Week[/], or [red]Exit[/] to main menu?")
            .AddChoices("[green]Today[/]", "[cyan]Pick a Day[/]", "[aqua]Past Week[/]", "[red]Exit[/]")));
    }

    public string GetAdminOption()
    {
        TrackerUtils.LineMessage("[red]Admin Options:[/]", "red3");

        var adminMenuChoices = new List<string>();

        if (DataStore.Instance.ContainsRecords()) adminMenuChoices.Add("Remove Last Update");
        if (DataStore.Instance.ContainsRecords()) adminMenuChoices.Add("Remove All Updates");
        if (!DataStore.Instance.HasDemoTrigger()) adminMenuChoices.Add("Add Demonstration Data");
        if (DataStore.Instance.HasDemoTrigger()) adminMenuChoices.Add("Remove Demonstration Data");

        adminMenuChoices.Add("Exit to Main Menu");

        return Markup.Remove(_console.Prompt(new SelectionPrompt<string>()
            .Title("[bold red]Select Admin Option:[/]")
            .AddChoices(adminMenuChoices)));
    }

    public DateTime PromptForDate()
    {
        var availableDates = DataStore.Instance.GetMoodRecords()
            .Select(r => r.Timestamp.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        if (availableDates.Count == 0)
        {
            TrackerUtils.CenteredMessage("[red]No mood records found![/]");
            return DateTime.Today;
        }

        var selection = new SelectionPrompt<DateTime>()
            .Title("Select a date to view reports:")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to see more dates)[/]")
            .UseConverter(d => d.ToString("dddd, MMMM d, yyyy"))
            .AddChoices(availableDates);

        return _console.Prompt(selection);
    }
}