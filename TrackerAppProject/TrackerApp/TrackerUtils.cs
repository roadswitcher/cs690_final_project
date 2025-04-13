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

    public static class ConsoleColor
    {
        public const string Emphasis = "green1";
        public const string Warning = "red";
        public const string Info = "blue";
        public const string Success = "green3";
        public const string Debug = "fuchsia";
    }

    public static void DebugMessage(string message, string color = ConsoleColor.Debug)
    {
#if DEBUG
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
#endif
    }

    // Reference link:  https://spectreconsole.net/appendix/colors
    public static void LineMessage(string message, string color = ConsoleColor.Info)
    {
        var rule = new Rule($"{message}").LeftJustified().RuleStyle(color);
        AnsiConsole.Write(rule);
    }

    public static void WarningMessageLeftJustified(string message, string warningcolor = ConsoleColor.Warning)
    {
        LineMessage(message, warningcolor);
    }

    public static bool ConfirmYesNo(string color = ConsoleColor.Info, bool defaultChoice = false)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<bool>("Please confirm yes/no: ")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(defaultChoice)
                .WithConverter(choice => choice ? "y" : "n")
                .PromptStyle(color)
        );
    }

    public static void CenteredMessage(string message, string color = ConsoleColor.Info)
    {
        var rule = new Rule($"{message}").Centered().RuleStyle(color);
        AnsiConsole.Write(rule);
    }

    public static void WarningMessageCentered(string message)
    {
        CenteredMessage(message, "red");
    }

    public static void ShowSelectedValue(string message, string color = ConsoleColor.Info)
    {
        var rule = new Rule($"You Selected: [green]{message}[/]").LeftJustified().RuleStyle(color);
        AnsiConsole.Write(rule);
    }

    public static void EnterToContinue(bool clearscreen = true,
        string textColor = ConsoleColor.Info,
        string ruleColor = ConsoleColor.Info)
    {
        var rule = new Rule($"[{textColor}]Please press Enter to continue[/]").Centered().RuleStyle(ruleColor);
        AnsiConsole.Write(rule);
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
        if (clearscreen) AnsiConsole.Clear();
    }

    public static void CenteredMessageEnterContinue(string message, string color = ConsoleColor.Emphasis,
        bool clearscreen = true)
    {
        var rule = new Rule($"{message} -- please select Enter to continue").Centered().RuleStyle(color);
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
        CenteredMessage(
            $"[{ConsoleColor.Emphasis}]MoodTracker[/] --- Logged in as [{ConsoleColor.Emphasis}]{userName}[/]");
        CenteredMessage($"Tracking [{ConsoleColor.Emphasis}]{DataStore.Instance.GetMoodRecordCount()}[/] Mood Updates");
    }

    public static void ShowExitMessages()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        CenteredMessage("[yellow]Thanks for using the app[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
    }
}