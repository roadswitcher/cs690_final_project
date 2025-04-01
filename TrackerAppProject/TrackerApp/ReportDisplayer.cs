using Spectre.Console;

namespace TrackerApp;

public class ReportDisplayer
{
    private readonly IAnsiConsole _console;

    public ReportDisplayer(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }
    
    public void DisplayDailyReport(DailyReport report)
{
    _console.Write(new Rule($"[cyan1]Daily Report for {report.Date:yyyy-MM-dd}[/]").LeftJustified().RuleStyle("cyan2"));
    
    // Show total records
    _console.WriteLine($"Total mood records: {report.TotalRecords}");
    
    // Show mood distribution
    if (report.TotalRecords > 0)
    {
        _console.WriteLine();
        _console.WriteLine("Mood Distribution:");
        var moodTable = new Table().Border(TableBorder.Simple);
        moodTable.AddColumn("Mood");
        moodTable.AddColumn("Count");
        moodTable.AddColumn("Percentage");
        
        foreach (var mood in report.MoodDistribution)
        {
            double percentage = (double)mood.Value / report.TotalRecords * 100;
            moodTable.AddRow(mood.Key, mood.Value.ToString(), $"{percentage:F1}%");
        }
        
        _console.Write(moodTable);
        
        // Show time of day distribution
        if (report.TimeOfDayDistribution?.Count > 0)
        {
            _console.WriteLine();
            _console.WriteLine("Time of Day Distribution:");
            var timeTable = new Table().Border(TableBorder.Simple);
            timeTable.AddColumn("Time of Day");
            timeTable.AddColumn("Count");
            timeTable.AddColumn("Percentage");
            
            foreach (var time in report.TimeOfDayDistribution)
            {
                double percentage = (double)time.Value / report.TotalRecords * 100;
                timeTable.AddRow(time.Key, time.Value.ToString(), $"{percentage:F1}%");
            }
            
            _console.Write(timeTable);
        }
    }
    else
    {
        _console.WriteLine("No mood records for past day.");
    }
    
    _console.WriteLine();
    _console.Prompt(new TextPrompt<string>("Press Enter to continue...").AllowEmpty());
}

    public void DisplayWeeklyReport(WeeklyReport report)
{
    DateTime startDate = report.Date.AddDays(-6);
    _console.Write(new Rule($"[cyan1]Weekly Report ({startDate:MMM dd} - {report.Date:MMM dd})[/]").LeftJustified().RuleStyle("cyan2"));
    
    // Show total records
    _console.WriteLine($"Total mood records: {report.TotalRecords}");
    
    if (report.TotalRecords > 0)
    {
        // Show mood distribution
        _console.WriteLine();
        _console.WriteLine("Mood Record Distribution:");
        var moodTable = new Table().Border(TableBorder.Simple);
        moodTable.AddColumn("Mood");
        moodTable.AddColumn("Count");
        moodTable.AddColumn("Percentage");
        
        foreach (var mood in report.MoodDistribution)
        {
            double percentage = (double)mood.Value / report.TotalRecords * 100;
            moodTable.AddRow(mood.Key, mood.Value.ToString(), $"{percentage:F1}%");
        }
        
        _console.Write(moodTable);
        
        // Show time of day distribution
        if (report.TimeOfDayDistribution?.Count > 0)
        {
            _console.WriteLine();
            _console.WriteLine("Time of Day Distribution:");
            var timeTable = new Table().Border(TableBorder.Simple);
            timeTable.AddColumn("Time of Day");
            timeTable.AddColumn("Count");
            timeTable.AddColumn("Percentage");
            
            foreach (var time in report.TimeOfDayDistribution)
            {
                double percentage = (double)time.Value / report.TotalRecords * 100;
                timeTable.AddRow(time.Key, time.Value.ToString(), $"{percentage:F1}%");
            }
            
            _console.Write(timeTable);
        }
        
        // Show daily breakdown
        if (report.DailyBreakdown?.Count > 0)
        {
            _console.WriteLine();
            _console.WriteLine("Daily Breakdown:");
            var dayTable = new Table().Border(TableBorder.Simple);
            dayTable.AddColumn("Day");
            dayTable.AddColumn("Count");
            dayTable.AddColumn("Percentage");
            
            // Order by day of week
            var orderedDays = report.DailyBreakdown
                .OrderBy(kv => (int)kv.Key)
                .ToList();
            
            foreach (var day in orderedDays)
            {
                double percentage = (double)day.Value / report.TotalRecords * 100;
                dayTable.AddRow(day.Key.ToString(), day.Value.ToString(), $"{percentage:F1}%");
            }
            
            _console.Write(dayTable);
        }
    }
    else
    {
        _console.WriteLine("No mood records for this week.");
    }
    
    _console.WriteLine();
    _console.Prompt(new TextPrompt<string>("Press Enter to continue...").AllowEmpty());
}

}