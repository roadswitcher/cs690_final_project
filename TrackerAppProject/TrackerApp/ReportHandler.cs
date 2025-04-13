namespace TrackerApp;

public class DailyReport
{
    public DateTime Date { get; init; }
    public int TotalRecords { get; init; }
    public Dictionary<string, int> MoodDistribution { get; init; } = new();
    public Dictionary<string, (int Count, string MostCommonMood)> TimeOfDayDistribution { get; init; } = new();

    public List<(string, string, string, string)> DailyBreakdown { get; init; } = [];
}

public class WeeklyReport
{
    public DateTime Date { get; init; }
    public int TotalRecords { get; init; }
    public Dictionary<string, int> MoodDistribution { get; init; } = new();
    public Dictionary<string, (int Count, string MostCommonMood)> TimeOfDayDistribution { get; init; } = new();
}

public class ReportHandler(IDataStore dataStore)
{
    // Give me a link to the DataStore singleton, or an exception if you cannot
    private readonly IDataStore _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));

    public DailyReport GetDailyReport(DateTime localDate)
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
            Date = localDate, TotalRecords = records.Count, MoodDistribution = GetMoodDistribution(records),
            TimeOfDayDistribution = GetTimeOfDayDistribution(records),
            DailyBreakdown = GetDailyBreakdownList(records)
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
            TimeOfDayDistribution = GetTimeOfDayDistribution(records)
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
private static Dictionary<string, (int Count, string MostCommonMood)> GetTimeOfDayDistribution(List<MoodRecord> records)
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
        foreach (var mood in TrackerUtils.MoodColors.Keys)
        {
            moodsByTimePeriod[period][mood] = 0;
        }
    }

    foreach (var record in records)
    {
        var localTime = record.Timestamp.ToLocalTime();
        var hour = localTime.Hour;
        
        string timeOfDay = hour switch
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
        if (moodFrequencies.Values.Sum() > 0) // Make sure there are records for this period
        {
            var mostCommonMood = moodFrequencies.OrderByDescending(kv => kv.Value).First().Key;
            distribution[period] = (distribution[period].Count, mostCommonMood);
        }
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
}