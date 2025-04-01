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

    public static void WelcomeScreen(string[] args)
    {
        AnsiConsole.Clear();

        DebugMessage($"Args Length: {args.Length}");
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