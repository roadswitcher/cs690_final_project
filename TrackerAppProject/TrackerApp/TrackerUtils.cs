using Spectre.Console;

namespace TrackerApp;

public static class TrackerUtils
{
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
        var rule = new Rule($"You Selected: [yellow]{message}[/]").LeftJustified().RuleStyle(color);
        AnsiConsole.Write(rule);
    }

    public static void WelcomeScreen(string[] args)
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(new FigletText("MoodApp").Centered().Color(Color.Green));
        DebugMessage($"Current Console Width: {AnsiConsole.Profile.Width}");

        CenteredMessage("[orange1]Welcome To MoodTracker![/]");
        CenteredMessage("Let's Get Started");

        var userCredentials = LoginHandler.HandleLogin();

        var dataStore = DataStore.Instance;
        dataStore.SetUserCredentials(userCredentials);

        AnsiConsole.MarkupLine($" Logged in as {userCredentials.Username}");
    }
}