using System;

public static class TrackerLauncher
{
    public static int Main(string[] args)
    {
        var app = new TrackerApp();
        return app.Run(args);
    }
}