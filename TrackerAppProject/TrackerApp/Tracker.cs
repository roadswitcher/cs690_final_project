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
        _reportDisplayer = new ReportDisplayer(console1, _dataStore);
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
        var today = DateTime.Now;

        switch (reportChoice)
        {
            case "Today":
                // var dailyReport = reportHandler.GenerateDailyReport(today);
                _reportDisplayer.DisplayDailyReport(today);
                break;
            case "Pick a Day":
                var chosenDate = _userInputHandler.PromptForDate();
                // var specificDayReport = reportHandler.GenerateDailyReport(chosenDate);
                _reportDisplayer.DisplayDailyReport(chosenDate);
                break;
            case "Past Week":
                // var weeklyReport = reportHandler.GeneratePriorWeekReport(today);
                _reportDisplayer.DisplayWeeklyReport();
                break;
            default:
                AnsiConsole.Clear();
                break;
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
                TrackerUtils.WarningMessageLeftJustified("PLEASE CONFIRM YOU WANT TO DELETE THE FOLLOWING UPDATE:");
                TrackerUtils.WarningMessageLeftJustified(
                    $"Time: {lastRecord.Timestamp.ToLocalTime()} / Mood:{lastRecord.Mood} / Trigger: {lastRecord.Trigger}");
                
                if (TrackerUtils.ConfirmYesNo()) _dataStore.RemoveLastMoodRecord();
                TrackerUtils.EnterToContinue();
                break;
            
            case "Remove All Data":
                TrackerUtils.WarningMessageCentered(
                    "Removing all data is an irreversible action.");
                TrackerUtils.WarningMessageCentered("PLEASE CONFIRM YOU WISH TO PROCEED");
                if (TrackerUtils.ConfirmYesNo()) _dataStore.RemoveAllMoodRecords();
                TrackerUtils.EnterToContinue();
                break;
            
            case "Add Demonstration Data":
                _dataStore.AddTheDemoData();
                TrackerUtils.CenteredMessageEnterContinue("Added Demonstration Data");
                break;
            
            case "Remove Demonstration Data":
                _dataStore.DeleteDemoData();
                TrackerUtils.CenteredMessageEnterContinue("Removed Demonstration data");
                break;
            
            default:
                AnsiConsole.Clear();
                break;
        }

        AnsiConsole.Clear();
    }
}