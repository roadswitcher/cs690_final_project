using Spectre.Console;

namespace TrackerApp;

internal class Tracker
{
    private readonly DataStore _dataStore;
    private readonly ReportDisplayer _reportDisplayer;
    private readonly List<string> _trackedEmotions;
    private readonly UserInputHandler _userInputHandler;

    public Tracker(IAnsiConsole console)
    {
        var console1 = console ?? throw new ArgumentNullException(nameof(console));
        _userInputHandler = new UserInputHandler(console1);
        _dataStore = DataStore.Instance;
        _trackedEmotions =
            ["Happy", "Sad", "Angry", "Wistful", "Indifferent", "Anxious", "Excited", "Frustrated", "Content"];

        _reportDisplayer = new ReportDisplayer(console1);
    }

    public int Run(string[] args)
    {
        TrackerUtils.WelcomeScreen(args);

        RunMoodTracker();

        TrackerUtils.ExitMessages();
        return 0;
    }

    private void RunMoodTracker()
    {
        var shouldExit = false;


        while (!shouldExit)
        {
            TrackerUtils.DisplayHeaderInfo();

            var choice = _userInputHandler.GetMainMenuChoice();

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
        var mood = _userInputHandler.GetMoodRecordUpdate(_trackedEmotions);
        _dataStore.AddMoodRecord(mood);
    }

    private void HandleReportGeneration()
    {
        var reportChoice = _userInputHandler.GetReportChoice();

        var reportHandler = new ReportHandler(_dataStore);
        var today = DateTime.Now;

        switch (reportChoice)
        {
            case "Today":
                var dailyReport = reportHandler.GetDailyReport(today);
                _reportDisplayer.DisplayDailyReport(dailyReport);
                break;
            case "Pick a Day":
                var chosenDate = _userInputHandler.PromptForDate();
                var specificDayReport = reportHandler.GetDailyReport(chosenDate);
                _reportDisplayer.DisplayDailyReport(specificDayReport);
                break;
            case "Week":
                var weeklyReport = reportHandler.GetWeeklyReport(today);
                _reportDisplayer.DisplayWeeklyReport(weeklyReport);
                break;
            case "Exit":
                // AnsiConsole.Clear();
                return;
        }
    }

    private void HandleAdminOptions()
    {
        var adminChoice = _userInputHandler.GetAdminOption();

        switch (adminChoice)
        {
            // implement choices
            case "Remove Last Update":
                var lastRecord = _dataStore.GetLastMoodRecord();
                TrackerUtils.LineMessage("Please confirm you want to remove this update:");
                TrackerUtils.LineMessage(
                    $"Time: {lastRecord.Timestamp.ToLocalTime().ToShortTimeString()} / Last Mood: {lastRecord.Mood} / Last Trigger: \"{lastRecord.Trigger}\"");
                var confirmation = AnsiConsole.Prompt(
                    new TextPrompt<bool>("Are you sure you want to delete that? [[Default: n]]")
                        .AddChoice(true)
                        .AddChoice(false)
                        .DefaultValue(false)
                        .WithConverter(choice => choice ? "y" : "n"));

                // Echo the confirmation back to the terminal
                Console.WriteLine(confirmation ? "Deletion Confirmed" : "Very well then.");
                if (confirmation) _dataStore.RemoveLastMoodRecord();
                break;
            case "Return to Main Menu":
                AnsiConsole.Clear();
                return;
        }

        TrackerUtils.EnterToContinue();
    }
}