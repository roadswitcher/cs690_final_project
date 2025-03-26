using Spectre.Console;

namespace TrackerApp
{
    internal class Tracker
    {
        private readonly DataStore _dataStore;
        private readonly UserInputHandler _userInputHandler;
        private readonly List<string> _trackedEmotions;

        public Tracker(IAnsiConsole console)
        {
            IAnsiConsole console1 = console ?? throw new ArgumentNullException(nameof(console));
            _userInputHandler = new UserInputHandler(console1);
            _dataStore = DataStore.Instance;
            _trackedEmotions = ["Happy", "Sad", "Mad", "Wistful", "Indifferent"];
            
        }

        public int Run(string[] args)
        {
            TrackerUtils.WelcomeScreen(args);

            RunMoodTracker();
            return 0;
        }

        private void RunMoodTracker()
        {
            bool shouldExit = false;

            while (!shouldExit)
            {
                List<string> trackedEmotions = ["Happy", "Sad", "Mad", "Wistful", "Indifferent"];

                object userInput = _userInputHandler.GetUserInput(trackedEmotions);

                switch (userInput)
                {
                    case string choice:
                        Console.WriteLine($"User picked: {choice}");
                        shouldExit = string.Equals(choice, "Exit", StringComparison.OrdinalIgnoreCase);
                        break;
                    case MoodRecord mood:
                        Console.WriteLine($"Mood: {mood.Mood}");
                        Console.WriteLine($"Trigger: {mood.Trigger}");
                        _dataStore.AddMoodRecord(mood);
                        TrackerUtils.DebugMessage(
                            $" *** Record count: {_dataStore.GetMoodRecordCount()}   Adding mood: {mood.Mood} ***");
                        break;
                }
            }
        }
    }
}