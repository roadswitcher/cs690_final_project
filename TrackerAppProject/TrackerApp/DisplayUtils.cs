using Spectre.Console;
using System;

namespace TrackerApp
{
    public static class TrackerUtils
    {
        public static void WelcomeScreen(string[] args)
        {
            AnsiConsole.Clear();

#if DEBUG
            var consoleWidth = AnsiConsole.Profile.Width;
            AnsiConsole.MarkupLine($"[bold yellow]Current Console Width: {consoleWidth}[/]");
            AnsiConsole.MarkupLine($"[bold yellow]Args Length: {args.Length}[/]");
#endif

            // AnsiConsole.Write(
            //     new FigletText("MoodTracker")
            //         .Centered()
            //         .Color(Color.Cyan1)
            // );

            AnsiConsole.Write(new Rule("[cyan1]Welcome To MoodTracker[/]").Centered().RuleStyle("green"));
            AnsiConsole.Write(
                new Rule("[bold green]Let's Get Started[/]")
                    .Centered()
                    .RuleStyle("green")
            );
        }
    }
}