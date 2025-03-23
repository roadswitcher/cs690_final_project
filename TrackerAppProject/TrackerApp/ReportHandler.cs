namespace TrackerApp
{
    public class DailyReport
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
            var records = _dataStore.GetMoodRecords().Where(record => record.Timestamp.Date == date).ToList();

            var report = new DailyReport { Date = date, TotalRecords = records.Count };
            
            return report;
        }

    }


}