using Spectre.Console;

namespace TrackerApp;

internal class Tracker
{
    private readonly DataStore _dataStore;
    private readonly List<string> _trackedEmotions;
    private readonly UserInputHandler _userInputHandler;
    private readonly ReportDisplayer _reportDisplayer;

    public Tracker(IAnsiConsole console)
    {
        var console1 = console ?? throw new ArgumentNullException(nameof(console));
        _userInputHandler = new UserInputHandler(console1);
        _dataStore = DataStore.Instance;
        _trackedEmotions = ["Happy", "Sad", "Mad", "Wistful", "Indifferent"];
        _reportDisplayer = new ReportDisplayer(console1);
    }

    public int Run(string[] args)
    {
        TrackerUtils.WelcomeScreen(args);

        RunMoodTracker();
        return 0;
    }

    private void RunMoodTracker()
    {
        var shouldExit = false;

        while (!shouldExit)
        {
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

        if (reportChoice == "Exit")
            return;

        var reportHandler = new ReportHandler(_dataStore);
        var today = DateTime.Now;

        switch (reportChoice)
        {
            case "Day":
                var dailyReport = reportHandler.GetDailyReport(today);
                _reportDisplayer.DisplayDailyReport(dailyReport);
                break;
            case "Week":
                var weeklyReport = reportHandler.GetWeeklyReport(today);
                _reportDisplayer.DisplayWeeklyReport(weeklyReport);
                break;
            // case "Month":
            //         MonthlyReport monthlyReport = reportHandler.GetMonthlyReport(today);
            //         _reportDisplayer.DisplayMonthlyReport(monthlyReport);
            //         break;
        }
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