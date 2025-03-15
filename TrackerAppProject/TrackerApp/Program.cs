using Spectre.Console;
using System;

namespace TrackerApp
{
    public static class TrackerLauncher
    {
        public static int Main(string[] args)
        {
            var app = new Tracker(AnsiConsole.Console);
            return app.Run(args);
        }
    }
}