using Spectre.Console;

namespace TrackerApp;

public class ReportDisplayer(IAnsiConsole console)
{
    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));

    private static readonly Dictionary<string, Color> _moodColors = new Dictionary<string, Color>
    {
        // color listing sourced from online research/taking first AI suggestions
        { "Happy", Color.Green1 }, // Bright green - universally associated with happiness
        { "Sad", Color.Blue3 }, // Darker blue - commonly associated with sadness
        { "Angry", Color.Red1 }, // Bright red - standard color for anger
        { "Wistful", Color.Purple }, // Soft purple - captures the reflective nature of wistfulness
        { "Indifferent", Color.Grey }, // Grey - represents neutrality/lack of strong emotion
        { "Anxious", Color.Yellow1 }, // Yellow - often used to indicate caution/nervousness
        { "Excited", Color.Orange1 }, // Vibrant orange - energetic and enthusiastic
        { "Frustrated", Color.Maroon }, // Maroon - a muted red showing intensity without being pure anger
        { "Content", Color.Cyan1 } // Calm blue-green - peaceful and satisfied
    };


    private static BreakdownChart BuildBreakdownChart(Dictionary<string, int> distribution)
    {
        var total = distribution.Values.Sum();

        var chart = new BreakdownChart()
            .FullSize().HideTagValues();


        var colorIndex = 0;

        foreach (var kvp in distribution)
        {
            var percentage = kvp.Value / (double)total * 100;
            chart.AddItem(kvp.Key, percentage, _moodColors[kvp.Key]);
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
            moodTable.Title = new TableTitle("Mood Distribution");
            foreach (var mood in report.MoodDistribution)
            {
                var percentage = (double)mood.Value / report.TotalRecords * 100;
                moodTable.AddRow(mood.Key, mood.Value.ToString(), $"{percentage:F1}%");
            }

            // var timeTable = new Table().Border(TableBorder.Simple);
            // timeTable.AddColumn("Time of Day");
            // timeTable.AddColumn("Count");
            // timeTable.AddColumn("Percentage");
            // timeTable.Title = new TableTitle("Time of Day Report Breakdown");
            //
            // if (report.TimeOfDayDistribution?.Count > 0)
            //     foreach (var time in report.TimeOfDayDistribution)
            //     {
            //         var percentage = (double)time.Value / report.TotalRecords * 100;
            //         timeTable.AddRow(time.Key, time.Value.ToString(), $"{percentage:F1}%");
            //     }

            var dailyBreakdown = report.DailyBreakdown;

            var breakdownTable = new Table();
            breakdownTable.AddColumn("Period");
            breakdownTable.AddColumn("Time");
            breakdownTable.AddColumn("Mood");
            breakdownTable.AddColumn("Triggers/Info");
            foreach (var (timeCategory, time, mood, trigger) in dailyBreakdown)
            {
                breakdownTable.AddRow(timeCategory, time, mood, trigger);
            }

            _console.Write(chart);
            _console.Write(moodTable);
            // _console.Write(timeTable);
            _console.Write(breakdownTable);
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
        }
        else
        {
            _console.WriteLine("No mood records for this week.");
        }

        TrackerUtils.EnterToContinue();
    }
}