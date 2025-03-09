using Spectre.Console;
using System;

namespace TrackerApp
{
    public static class TrackerUtils
    {
        public static void ScreamPrint(string message)
        {
            // Utility print function for messages used during DEBUG mode
            #if DEBUG
            AnsiConsole.MarkupLine($"[bold yellow]{message}[/]");
            #else
            // Noop
            #endif
        }
        public static void WelcomeScreen(string[] args)
        {
            //AnsiConsole.Clear();
            
            int consoleWidth = AnsiConsole.Profile.Width;
            ScreamPrint($"Console Width: {consoleWidth}");
            ScreamPrint($"Args length: {args.Length}");


            AnsiConsole.Write(
                new FigletText("Welcome!")
                    .Centered()
                    .Color(Color.Cyan1)
            );

            AnsiConsole.Write(
                new Rule("[bold green]Let's Get Started[/]")
                    .Centered()
                    .RuleStyle("green")
            );
        }
    }
}