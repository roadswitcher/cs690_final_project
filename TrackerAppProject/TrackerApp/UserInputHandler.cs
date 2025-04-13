using Spectre.Console;
using TrackerApp.ObjectClasses;

namespace TrackerApp;
using msgColors = TrackerUtils.MsgColors;

public class UserInputHandler(IAnsiConsole console)
{
   
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));

    public string GetMainMenuChoice()
    {
        return Markup.Remove(_console.Prompt(new SelectionPrompt<string>()
            .Title("[green]Update[/] mood, [aqua]Report[/] data, [red]Admin Options[/] or [red]Exit[/] app?")
            .AddChoices("[green]Update[/]", "[aqua]Report[/]", "[red]Admin Options[/]", "[red]Exit[/]")
            .HighlightStyle(msgColors.Query)
        )
        );
    }

    public MoodRecord GetMoodRecordUpdate(List<string> trackedEmotions)
    {
        TrackerUtils.LineMessage($"[{msgColors.Emphasis}]Mood Update:[/] [{msgColors.Query}]What is your current mood?[/]");
        var mood = _console.Prompt(new SelectionPrompt<string>()
            .AddChoices(trackedEmotions).HighlightStyle(msgColors.Query));
        TrackerUtils.LineMessage($"[{msgColors.Emphasis}]You chose: {mood}[/]");

        TrackerUtils.LineMessage(
            $"[{msgColors.Emphasis}]External Factors:[/] [{msgColors.Query}]Do you want to record more information, like triggers or external factors?[/]");

        var trigger =
            _console.Prompt(
                new TextPrompt<string>($"[{msgColors.Query}][[Optional]] (hit Enter):[/]")
                    .AllowEmpty());

        var triggerPresent = !string.IsNullOrEmpty(trigger);

        var moodRecord = new MoodRecord(mood, trigger);
        var localtime = moodRecord.Timestamp.ToLocalTime().ToShortTimeString();

        TrackerUtils.LineMessage(
            triggerPresent
                ? $"Saving update:  Time [{msgColors.Emphasis}]{localtime}[/], Mood [{msgColors.Emphasis}]{mood}[/], with additional note: [{msgColors.Emphasis}]{trigger}[/]"
                : $"Saving update:  Time [{msgColors.Emphasis}]{localtime}[/], Mood [{msgColors.Emphasis}]{mood}[/], no additional factors");

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
        TrackerUtils.WarningMessageLeftJustified("Admin Options:");

        var adminMenuChoices = new List<string>();

        if (DataStore.Instance.ContainsRecords()) adminMenuChoices.Add("Remove Last Update");
        if (DataStore.Instance.ContainsRecords()) adminMenuChoices.Add("Remove All Data");
        if (!DataStore.Instance.HasDemoTrigger()) adminMenuChoices.Add("Add Demonstration Data");
        if (DataStore.Instance.HasDemoTrigger()) adminMenuChoices.Add("Remove Demonstration Data");

        adminMenuChoices.Add("Exit to Main Menu");

        return Markup.Remove(
            _console.Prompt(new SelectionPrompt<string>()
            .Title($"[{msgColors.Warning}]Select Admin Option:[/]").AddChoices(adminMenuChoices).HighlightStyle(msgColors.Query))
        );
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
            TrackerUtils.WarningMessageCentered("No mood records found!");
            return DateTime.Today;
        }

        var selection = new SelectionPrompt<DateTime>()
            .Title("Select a date to view reports:")
            .PageSize(10)
            .MoreChoicesText($"[{msgColors.Query}](Move up and down to see more dates)[/]")
            .UseConverter(d => d.ToString("dddd, MMMM d, yyyy"))
            .AddChoices(availableDates).HighlightStyle(msgColors.Query);

        return _console.Prompt(selection);
    }
}