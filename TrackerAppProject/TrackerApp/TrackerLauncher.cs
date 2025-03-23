using Spectre.Console;

namespace TrackerApp
{
    public static class TrackerLauncher
    {
        public static int Main(string[] args)
        {
            Tracker app = new(AnsiConsole.Console);
            return app.Run(args);
        }
    }
}