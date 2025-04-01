namespace TrackerApp
{
    public class DailyReport
    {
        public DateTime Date { get; init; }
        public int TotalRecords { get; init; }
        public Dictionary<string, int> MoodDistribution { get; set; } = new();
        // public Dictionary<string, int> TimeOfDayDistribution { get; set; } = new();
    }

    public class WeeklyReport
    {
        public DateTime Date { get; init; }
        public int TotalRecords { get; init; }
        public Dictionary<string, int> MoodDistribution { get; set; } = new();
        // public Dictionary<string, int> TimeOfDayDistribution { get; set; } = new();
        // public Dictionary<DayOfWeek, int> DailyBreakdown { get; set; } = new();
    }

    public class MonthlyReport
    {
        public DateTime Date { get; init; }
        public int TotalRecords { get; init; }
    }


    public class ReportHandler(IDataStore dataStore)
    {
        // Give me a link to the DataStore singleton, or an exception if you cannot
        private readonly IDataStore _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));

        // TODO
        // - Daily report
        // - Weekly Report
        // - Monthly Report

        public DailyReport GetDailyReport(DateTime date)
        {
            date = date.Date;
            List<MoodRecord> records =
                _dataStore.GetMoodRecords().Where(record => record.Timestamp.Date == date).ToList();

            var report = new DailyReport
            {
                Date = date, TotalRecords = records.Count, MoodDistribution = GetMoodDistribution(records)
            };
            return report;
        }

        public WeeklyReport GetWeeklyReport(DateTime today)
        {
            DateTime weekAgo = today.Date.AddDays(-6);
            List<MoodRecord> records = _dataStore.GetMoodRecords()
                .Where(record => record.Timestamp.Date >= weekAgo && record.Timestamp.Date <= today).ToList();
            WeeklyReport report = new() { Date = DateTime.Now, TotalRecords = records.Count };
            return report;
        }

        
        public MonthlyReport GetMonthlyReport(DateTime today)
        {
            DateTime monthAgo = today.Date.AddMonths(-1);
            List<MoodRecord> records = _dataStore.GetMoodRecords()
                .Where(record => record.Timestamp.Date >= monthAgo && record.Timestamp.Date <= today).ToList();
            MonthlyReport report = new() { Date = DateTime.Now, TotalRecords = records.Count };
            return report;
        }
        
        private Dictionary<string, int> GetMoodDistribution(List<MoodRecord> records)
        {
            var distribution = new Dictionary<string, int>();
            
            foreach (var record in records)
            {
                if (!distribution.ContainsKey(record.Mood))
                {
                    distribution[record.Mood] = 0;
                }
                
                distribution[record.Mood]++;
            }
            
            return distribution;
        }
        
        private Dictionary<string, int> GetTimeOfDayDistribution(List<MoodRecord> records)
        {
            var distribution = new Dictionary<string, int>
            {
                { "Morning", 0 },
                { "Afternoon", 0 },
                { "Evening", 0 },
                { "Night", 0 }
            };
            
            foreach (var record in records)
            {
                int hour = record.Timestamp.Hour;
                
                string timeOfDay = hour switch
                {
                    >= 5 and < 12 => "Morning",
                    >= 12 and < 17 => "Afternoon",
                    >= 17 and < 21 => "Evening",
                    _ => "Night"
                };
                
                distribution[timeOfDay]++;
            }
            
            return distribution;
        }

    }
}