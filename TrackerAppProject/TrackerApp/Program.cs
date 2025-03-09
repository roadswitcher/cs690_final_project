using Spectre.Console;
using System;

public static class TrackerLauncher
{
    public static int Main(string[] args)
    {
        var app = new TrackerApp(AnsiConsole.Console);
        return app.Run(args);
    }
}