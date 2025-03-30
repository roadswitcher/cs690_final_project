using Spectre.Console;

namespace TrackerApp
{
    internal class Tracker
    {
        private readonly DataStore _dataStore;
        private readonly List<string> _trackedEmotions;
        private readonly UserInputHandler _userInputHandler;

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
                string choice = _userInputHandler.GetMainMenuChoice();

                switch (choice)
                {
                    case "Update":
                        HandleMoodUpdate();
                        break;
                    case "Report":
                        HandleReportGeneration();
                        break;
                    case "Admin Options":
                        HandleAdminOptions();
                        break;
                    case "Exit":
                        shouldExit = true;
                        break;
                }
            }
        }

        private void HandleMoodUpdate()
        {
            MoodRecord mood = _userInputHandler.GetMoodRecordUpdate(_trackedEmotions);
            _dataStore.AddMoodRecord(mood);
        }

        private void HandleReportGeneration()
        {
            Console.WriteLine("Not yet implemented");
        }

        private void HandleAdminOptions()
        {
            // string adminChoice = _userInputHandler.GetAdminOption();
            Console.WriteLine("Not yet implemented");
            // switch (adminChoice):
            //
            // {
            //     // implement choices
            // }
        }
    }
}