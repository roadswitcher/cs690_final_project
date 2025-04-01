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

        AnsiConsole.Write(new Rule("[cyan1]Welcome To MoodTracker[/]").Centered().RuleStyle("green"));
        AnsiConsole.Write(
            new Rule("[bold green]Let's Get Started[/]")
                .Centered()
                .RuleStyle("green")
        );

        var userCredentials = LoginHandler.HandleLogin();

        var dataStore = DataStore.Instance;
        dataStore.SetUserCredentials(userCredentials);

        AnsiConsole.MarkupLine($" Logged in as {userCredentials.Username}");
        AnsiConsole.WriteLine();
    }
}