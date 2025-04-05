using Spectre.Console;

namespace TrackerApp;

public static class TrackerUtils
{
    public static readonly Dictionary<string, Color> MoodColors = new()
    {
        // color listing sourced from online research/taking first AI suggestions
        { "Happy", Color.Green1 }, // Bright green - universally associated with happiness
        { "Sad", Color.Blue3 }, // Darker blue - commonly associated with sadness
        { "Angry", Color.Red1 }, // Bright red - standard color for anger
        { "Wistful", Color.Purple }, // Soft purple - captures the reflective nature of wistfulness
        { "Indifferent", Color.Grey }, // Grey - represents neutrality/lack of strong emotion
        { "Anxious", Color.Yellow1 }, // Yellow - often used to indicate caution/nervousness
        { "Excited", Color.Orange1 }, // Vibrant orange - energetic and enthusiastic
        { "Frustrated", Color.Maroon }, // Maroon - a muted red showing intensity without being pure anger
        { "Content", Color.Cyan1 } // Calm blue-green - peaceful and satisfied
    };

    public static void DebugMessage(string message)
    {
#if DEBUG
        AnsiConsole.MarkupLine($"[bold yellow]{message}[/]");
#endif
    }

    // Reference link:  https://spectreconsole.net/appendix/colors
    public static void LineMessage(string message, string color = "orange1")
    {
        var rule = new Rule($"{message}").LeftJustified().RuleStyle(color);
        AnsiConsole.Write(rule);
    }

    public static void CenteredMessage(string message, string color = "green")
    {
        var rule = new Rule($"{message}").Centered().RuleStyle(color);
        AnsiConsole.Write(rule);
    }

    public static void ShowSelectedValue(string message, string color = "orange1")
    {
        var rule = new Rule($"You Selected: [yellow]{message}[/]").LeftJustified().RuleStyle(color);
        AnsiConsole.Write(rule);
    }

    public static void ShowEnteredValue(string message, string color = "orange1")
    {
        var rule = new Rule($"You Entered: [yellow]{message}[/]").LeftJustified().RuleStyle(color);
        AnsiConsole.Write(rule);
    }

    public static void EnterToContinue(bool clearscreen = true)
    {
        var rule = new Rule("[yellow]Please press Enter to continue[/]").Centered().RuleStyle("yellow");
        AnsiConsole.Write(rule);
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
        if (clearscreen) AnsiConsole.Clear();
    }

    public static void WelcomeScreen(string[] args)
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(new FigletText("MoodTracker").Centered().Color(Color.Green));
        DebugMessage($"Current Console Width: {AnsiConsole.Profile.Width}");

        var userCredentials = LoginHandler.HandleLogin();

        var dataStore = DataStore.Instance;
        dataStore.SetUserCredentials(userCredentials);
    }

    public static void DisplayHeaderInfo()
    {
        var userName = DataStore.Instance.GetUserCredentials().Username;
        CenteredMessage($"[orange3]MoodTracker[/] --- Logged in as [orange3]{userName}[/]");
        CenteredMessage($"Tracking [orange3]{DataStore.Instance.GetMoodRecordCount()}[/] Mood Updates");
    }

    public static void ExitMessages()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        CenteredMessage("[orange3]Thanks for using the app[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }
}