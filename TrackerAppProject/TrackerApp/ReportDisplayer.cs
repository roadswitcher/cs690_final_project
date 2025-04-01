using System;
using System.Collections.Generic;
using Spectre.Console;

namespace TrackerApp;

public class ReportDisplayer(IAnsiConsole console)
{
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));

    private static BreakdownChart BuildBreakdownChart(Dictionary<string, int> distribution)
    {
        var total = distribution.Values.Sum();

        var chart = new BreakdownChart()
            .FullSize().HideTagValues();

        // Try and make sure adjacent colors are distinct
        var colors = new List<Color> { Color.Red1, Color.Lime, Color.Blue1, Color.Yellow1, Color.Fuchsia, Color.Green3, Color.Orange1, Color.Green1 };

        var colorIndex = 0;

        foreach (var kvp in distribution)
        {
            double percentage = (kvp.Value / (double)total) * 100;
            chart.AddItem(kvp.Key, percentage, colors[colorIndex % colors.Count]);
            colorIndex++;
        }

        return chart;
    }

    public void DisplayDailyReport(DailyReport report)
{
    _console.Write(new Rule($"[cyan1]Daily Report for {report.Date:yyyy-MM-dd}[/]").LeftJustified()
        .RuleStyle("cyan2"));
    _console.WriteLine($"Number of updates today: {report.TotalRecords}");
    
    // Show mood distribution
    if (report.TotalRecords > 0)
    {
        var chart = BuildBreakdownChart(report.MoodDistribution).Width(75);
        
        var moodTable = new Table().Border(TableBorder.Simple);
        moodTable.AddColumn("Mood");
        moodTable.AddColumn("Count");
        moodTable.AddColumn("Percentage");
        moodTable.Title = new TableTitle("Mood Record Distribution");
        foreach (var mood in report.MoodDistribution)
        {
            var percentage = (double)mood.Value / report.TotalRecords * 100;
            moodTable.AddRow(mood.Key, mood.Value.ToString(), $"{percentage:F1}%");
        }
        
        var timeTable = new Table().Border(TableBorder.Simple);
        timeTable.AddColumn("Time of Day");
        timeTable.AddColumn("Count");
        timeTable.AddColumn("Percentage");
        timeTable.Title = new TableTitle("Time of Day Distribution");
        
        if (report.TimeOfDayDistribution?.Count > 0)
        {
            foreach (var time in report.TimeOfDayDistribution)
            {
                var percentage = (double)time.Value / report.TotalRecords * 100;
                timeTable.AddRow(time.Key, time.Value.ToString(), $"{percentage:F1}%");
            }
        } ;
        
        _console.Write(chart);
        _console.Write(moodTable);
        _console.Write(timeTable);
    }
    else
    {
        _console.WriteLine("No mood records for past day.");
    }
    TrackerUtils.EnterToContinue();
}
    public void DisplayWeeklyReport(WeeklyReport report)
    {
        var startDate = report.Date.AddDays(-6);
        _console.Write(new Rule($"[cyan1]Weekly Report ({startDate:MMM dd} - {report.Date:MMM dd})[/]").LeftJustified()
            .RuleStyle("cyan2"));

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
                var percentage = (double)mood.Value / report.TotalRecords * 100;
                moodTable.AddRow(mood.Key, mood.Value.ToString(), $"{percentage:F1}%");
            }

            _console.Write(BuildBreakdownChart(report.MoodDistribution));
            _console.Write(moodTable);

            // Show time of day distribution
            if (report.TimeOfDayDistribution?.Count > 0)
            {
                _console.WriteLine();
                _console.WriteLine("Time of Day Distribution for Reports:");
                var timeTable = new Table().Border(TableBorder.Simple);
                timeTable.AddColumn("Time of Day");
                timeTable.AddColumn("Count");
                timeTable.AddColumn("Percentage");

                foreach (var time in report.TimeOfDayDistribution)
                {
                    var percentage = (double)time.Value / report.TotalRecords * 100;
                    timeTable.AddRow(time.Key, time.Value.ToString(), $"{percentage:F1}%");
                }

                _console.Write(timeTable);
            }

            if (!(report.DailyBreakdown?.Count > 0)) return;
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
                    var percentage = (double)day.Value / report.TotalRecords * 100;
                    dayTable.AddRow(day.Key.ToString(), day.Value.ToString(), $"{percentage:F1}%");
                }

                _console.Write(dayTable);
            }
        }
        else
        {
            _console.WriteLine("No mood records for this week.");
        }
        TrackerUtils.EnterToContinue();
    }
}