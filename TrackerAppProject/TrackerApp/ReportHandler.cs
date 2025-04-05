namespace TrackerApp;

public class DailyReport
{
    public DateTime Date { get; init; }
    public int TotalRecords { get; init; }
    public Dictionary<string, int> MoodDistribution { get; init; } = new();
    public Dictionary<string, int> TimeOfDayDistribution { get; init; } = new();
}

public class WeeklyReport
{
    public DateTime Date { get; init; }
    public int TotalRecords { get; init; }
    public Dictionary<string, int> MoodDistribution { get; init; } = new();
    public Dictionary<string, int> TimeOfDayDistribution { get; init; } = new();
    // public Dictionary<DayOfWeek, int> DailyBreakdown { get; init; } = new();
}

public class ReportHandler(IDataStore dataStore)
{
    // Give me a link to the DataStore singleton, or an exception if you cannot
    private readonly IDataStore _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));

    public DailyReport GetDailyReport(DateTime date)
    {
        date = date.Date;
        var records =
            _dataStore.GetMoodRecords().Where(record => record.Timestamp.Date == date).ToList();

        var report = new DailyReport
        {
            Date = date, TotalRecords = records.Count, MoodDistribution = GetMoodDistribution(records),
            TimeOfDayDistribution = GetTimeOfDayDistribution(records)
        };
        return report;
    }

    public WeeklyReport GetWeeklyReport(DateTime today)
    {
        var weekAgo = today.Date.AddDays(-6);
        var records = _dataStore.GetMoodRecords()
            .Where(record => record.Timestamp.Date >= weekAgo && record.Timestamp.Date <= today).ToList();

        var report = new WeeklyReport
        {
            Date = today,
            TotalRecords = records.Count,
            MoodDistribution = GetMoodDistribution(records),
            TimeOfDayDistribution = GetTimeOfDayDistribution(records),
            // DailyBreakdown = GetDayOfWeekDistribution(records)
        };

        return report;
    }


    private static Dictionary<string, int> GetMoodDistribution(List<MoodRecord> records)
    {
        var distribution = new Dictionary<string, int>();

        foreach (var record in records)
        {
            distribution.TryAdd(record.Mood, 0);

            distribution[record.Mood]++;
        }

        return distribution;
    }

    private static Dictionary<string, int> GetTimeOfDayDistribution(List<MoodRecord> records)
    {
        var distribution = new Dictionary<string, int>
        {
            { "Morning", 0 },
            { "Afternoon", 0 },
            { "Evening", 0 },
            { "Night", 0 }
        };

        foreach (var timeOfDay in records.Select(record => record.Timestamp.ToLocalTime())
                     .Select(localTime => localTime.Hour).Select(hour => hour switch
                     {
                         >= 5 and < 12 => "Morning",
                         >= 12 and < 17 => "Afternoon",
                         >= 17 and < 21 => "Evening",
                         _ => "Night"
                     }))
            distribution[timeOfDay]++;

        return distribution;
    }
    
    private static Dictionary<string, int, >

    private static Dictionary<DayOfWeek, int> GetDayOfWeekDistribution(List<MoodRecord> records)
    {
        var distribution = new Dictionary<DayOfWeek, int>();

        foreach (var record in records)
        {
            var day = record.Timestamp.DayOfWeek;

            distribution.TryAdd(day, 0);

            distribution[day]++;
        }

        return distribution;
    }
}