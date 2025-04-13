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
        _trackedEmotions = TrackerUtils.MoodColors.Keys.ToList();
        _reportDisplayer = new ReportDisplayer(console1);
    }

    public int Run(string[] args)
    {
        TrackerUtils.WelcomeScreen(args);

        RunMoodTracker();

        TrackerUtils.ShowExitMessages();
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
                var dailyReport = reportHandler.GenerateDailyReport(today);
                _reportDisplayer.DisplayDailyReport(dailyReport);
                break;
            case "Pick a Day":
                var chosenDate = _userInputHandler.PromptForDate();
                var specificDayReport = reportHandler.GenerateDailyReport(chosenDate);
                _reportDisplayer.DisplayDailyReport(specificDayReport);
                break;
            case "Past Week":
                var weeklyReport = reportHandler.GeneratePriorWeekReport(today);
                _reportDisplayer.DisplayWeeklyReport(weeklyReport);
                break;
            case "Exit":
                AnsiConsole.Clear();
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
                TrackerUtils.WarningMessage("PLEASE CONFIRM YOU WANT TO DELETE THE FOLLOWING UPDATE:");
                TrackerUtils.WarningMessage(
                    $"Time: {lastRecord.Timestamp.ToLocalTime()} / Mood:{lastRecord.Mood} / Trigger: {lastRecord.Trigger}");
                var confirmation = TrackerUtils.ConfirmYesNo();

                // Echo the confirmation back to the terminal
                Console.WriteLine(confirmation ? "Deletion Confirmed" : "Very well then.");
                if (confirmation) _dataStore.RemoveLastMoodRecord();
                TrackerUtils.EnterToContinue();
                break;
            case "Remove All Data":
                TrackerUtils.CenteredMessage("Removing all data will force you to login again.");
                
                return;
            case "Add Demonstration Data":
                _dataStore.AddTheDemoData();
                TrackerUtils.CenteredMessageEnterContinue("Added Demonstration Data");
                return;
            case "Remove Demonstration Data":
                _dataStore.DeleteDemoData();
                TrackerUtils.CenteredMessageEnterContinue("Removed Demonstration data");
                return;
            case "Return to Main Menu":
                AnsiConsole.Clear();
                return;
        }

        AnsiConsole.Clear();
    }
}