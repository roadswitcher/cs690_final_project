namespace TrackerApp.ObjectClasses;

public class WeeklyReport
{
    public DateTime Date { get; init; }
    public int TotalRecords { get; init; }
    public Dictionary<string, int> MoodDistribution { get; init; } = new();
    public Dictionary<string, (int Count, string MostCommonMood)> TimeOfDayDistribution { get; init; } = new();
}