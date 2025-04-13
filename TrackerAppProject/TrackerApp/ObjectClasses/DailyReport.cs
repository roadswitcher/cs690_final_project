namespace TrackerApp.ObjectClasses;

public class DailyReport
{
    public DateTime Date { get; init; }
    public int TotalRecords { get; init; }
    public Dictionary<string, int> MoodDistribution { get; init; } = new();
    public Dictionary<string, (int Count, string MostCommonMood)> TimeOfDayDistribution { get; init; } = new();

    public List<(string, string, string, string)> DailyBreakdown { get; init; } = [];
}