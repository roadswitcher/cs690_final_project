using System;
using Spectre.Console;

public static class TrackerLauncher
{
    public static int Main(string[] args)
    {
        var app = new TrackerApp(AnsiConsole.Console);
        return app.Run(args);
    }
}