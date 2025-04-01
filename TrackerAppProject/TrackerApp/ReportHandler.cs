namespace TrackerApp;

public class DailyReport
{
    public DateTime Date { get; set; }
    public int TotalRecords { get; set; }
}

public class WeeklyReport
{
    public DateTime Date { get; set; }
    public int TotalRecords { get; set; }
}

public class ReportHandler(IDataStore dataStore)
{
    // Give me a link to the DataStore singleton, or an exception if you cannot
    private readonly IDataStore _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));

    // TODO
    // - Daily report ( aggregate, time of day )
    // - Weekly Report ( aggregate, time of day )
    // Stretch goal- Trends Identification?

    public DailyReport GetDailyReport(DateTime date)
    {
        date = date.Date;
        var records =
            _dataStore.GetMoodRecords().Where(record => record.Timestamp.Date == date).ToList();

        DailyReport report = new() { Date = date, TotalRecords = records.Count };

        return report;
    }

    public WeeklyReport GetWeeklyReport(DateTime today)
    {
        var weekAgo = today.Date.AddDays(-6);
        var records = _dataStore.GetMoodRecords()
            .Where(record => record.Timestamp.Date >= weekAgo && record.Timestamp.Date <= today).ToList();
        WeeklyReport report = new() { Date = DateTime.Now, TotalRecords = records.Count };
        return report;
    }
}