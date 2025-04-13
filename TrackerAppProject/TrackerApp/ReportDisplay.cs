using Spectre.Console;
using TrackerApp.ObjectClasses;

namespace TrackerApp;

public class ReportDisplay(IAnsiConsole console, IDataStore dataStore)
{
    private static readonly Dictionary<string, Color> MoodColors = TrackerUtils.MoodColors;
    private readonly IDataStore _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));

    private readonly IAnsiConsole _console = console ?? throw new ArgumentNullException(nameof(console));


    private static BreakdownChart BuildBreakdownChart(Dictionary<string, int> distribution)
    {
        var total = distribution.Values.Sum();

        var chart = new BreakdownChart()
            .FullSize().HideTagValues();


        foreach (var kvp in distribution)
        {
            var percentage = kvp.Value / (double)total * 100;
            var label = $"{kvp.Key} ({(int)Math.Round(percentage)}%)";
            chart.AddItem(label, percentage, MoodColors[kvp.Key]);
        }

        return chart;
    }

    public DailyReport GenerateDailyReport(DateTime localDate)
    {
        localDate = localDate.Date;

        // Convert to a UTC range to solve the 'midnight' problem with timezones
        var localStart = localDate;
        var localEnd = localDate.AddDays(1);

        var utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart);
        var utcEnd = TimeZoneInfo.ConvertTimeToUtc(localEnd);

        var records = _dataStore.GetMoodRecords()
            .Where(record => record.Timestamp >= utcStart && record.Timestamp < utcEnd)
            .ToList();

        var report = new DailyReport
        {
            Date = localDate, TotalRecords = records.Count,
            MoodDistribution = GetMoodDistributionFromListOfRecords(records),
            TimeOfDayDistribution = GetTimeOfDayDistributionFromListOfRecords(records),
            DailyBreakdown = GetDailyBreakdownList(records)
        };

        return report;
    }

    public void DisplayDailyReport(DateTime date)
    {
        var report = GenerateDailyReport(date);

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

            var dailyBreakdown = report.DailyBreakdown;

            var breakdownTable = new Table();
            breakdownTable.AddColumn("Period");
            breakdownTable.AddColumn("Time");
            breakdownTable.AddColumn("Mood");
            breakdownTable.AddColumn("Triggers/Info");
            foreach (var (timeCategory, time, mood, trigger) in dailyBreakdown)
                breakdownTable.AddRow(timeCategory, time, mood, trigger);

            _console.Write(chart);
            _console.Write(breakdownTable);
        }
        else
        {
            _console.WriteLine("No mood records for past day.");
        }
    }

    public void DisplayWeeklyReport()
    {
        var report = GeneratePriorWeekReport(DateTime.Now);
        var startDate = report.Date.AddDays(-6);
        AnsiConsole.Clear();

        _console.Write(
            new Rule(
                    $"[cyan1]Past Week Report: ({startDate:MMM dd} - {report.Date:MMM dd}) - {report.TotalRecords} mood updates[/]")
                .LeftJustified()
                .RuleStyle("cyan2"));

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
                timeTable.AddColumn("Most Common Mood");

                foreach (var time in report.TimeOfDayDistribution)
                {
                    var percentage = (double)time.Value.Count / report.TotalRecords * 100;
                    var moodDisplay = string.IsNullOrEmpty(time.Value.MostCommonMood)
                        ? "-"
                        : time.Value.MostCommonMood;
                    timeTable.AddRow(time.Key, time.Value.Count.ToString(), $"{percentage:F1}%", moodDisplay);
                }

                _console.Write(timeTable);
            }
        }
        else
        {
            _console.WriteLine("No mood records for this week.");
        }
    }

    private static Dictionary<string, (int Count, string MostCommonMood)> GetTimeOfDayDistributionFromListOfRecords(
        List<MoodRecord> records)
    {
        var timePeriods = new[]
        {
            "Morning",
            "Midday",
            "Afternoon",
            "Evening",
            "Night"
        };

        // Initialize the distribution dictionary with zero counts and empty most common moods
        var distribution = timePeriods.ToDictionary(
            period => period,
            _ => (Count: 0, MostCommonMood: ""));

        // Track mood frequencies for each time period
        var moodsByTimePeriod = new Dictionary<string, Dictionary<string, int>>();

        foreach (var period in timePeriods)
        {
            moodsByTimePeriod[period] = new Dictionary<string, int>();
            foreach (var mood in TrackerUtils.MoodColors.Keys) moodsByTimePeriod[period][mood] = 0;
        }

        foreach (var record in records)
        {
            var localTime = record.Timestamp.ToLocalTime();
            var hour = localTime.Hour;

            var timeOfDay = hour switch
            {
                >= 5 and < 11 => "Morning",
                >= 11 and < 13 => "Midday",
                >= 13 and < 17 => "Afternoon",
                >= 17 and < 21 => "Evening",
                _ => "Night"
            };

            // Increment the count for this time period and track mood frequency
            distribution[timeOfDay] = (distribution[timeOfDay].Count + 1, distribution[timeOfDay].MostCommonMood);
            moodsByTimePeriod[timeOfDay][record.Mood]++;
        }

        foreach (var period in timePeriods)
        {
            var moodFrequencies = moodsByTimePeriod[period];
            if (moodFrequencies.Values.Sum() <= 0) continue; // Make sure there are records for this period
            var mostCommonMood = moodFrequencies.OrderByDescending(kv => kv.Value).First().Key;
            distribution[period] = (distribution[period].Count, mostCommonMood);
        }

        return distribution;
    }

    private static List<(string TimeCategory, string Time, string Mood, string Trigger)> GetDailyBreakdownList(
        List<MoodRecord> records)
    {
        var sortedRecords = records.OrderBy(r => r.Timestamp).ToList();

        var tableData = sortedRecords.Select(record =>
        {
            var localTime = record.Timestamp.ToLocalTime();
            var time = localTime.ToString("HH:mm");

            var timeCategory = localTime.Hour switch
            {
                >= 5 and < 12 => "Morning",
                >= 12 and < 17 => "Afternoon",
                >= 17 and < 21 => "Evening",
                _ => "Night"
            };

            var mood = record.Mood;
            var trigger = string.IsNullOrEmpty(record.Trigger) ? "-" : record.Trigger;

            return (timeCategory, time, mood, trigger);
        }).ToList();

        return tableData;
    }

    public WeeklyReport GeneratePriorWeekReport(DateTime today)
    {
        var weekAgo = today.Date.AddDays(-6);
        var records = _dataStore.GetMoodRecords()
            .Where(record => record.Timestamp.Date >= weekAgo && record.Timestamp.Date <= today).ToList();

        var report = new WeeklyReport
        {
            Date = today,
            TotalRecords = records.Count,
            MoodDistribution = GetMoodDistributionFromListOfRecords(records),
            TimeOfDayDistribution = GetTimeOfDayDistributionFromListOfRecords(records)
        };

        return report;
    }


    private static Dictionary<string, int> GetMoodDistributionFromListOfRecords(List<MoodRecord> records)
    {
        var distribution = new Dictionary<string, int>();

        foreach (var record in records)
        {
            distribution.TryAdd(record.Mood, 0);

            distribution[record.Mood]++;
        }

        return distribution;
    }
}