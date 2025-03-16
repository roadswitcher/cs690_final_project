namespace TrackerApp
{
    public class AppData
    {
        public UserCreds UserCredentials { get; set; } = new UserCreds();
        public List<MoodRecord> MoodRecords { get; set; } = new List<MoodRecord>();
    }

    public class UserCreds
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
    
    public class DataStore
    {
        private static DataStore _instance;
        private static readonly object _lock = new object();
        private string _databaseFilePath;
        private List<MoodRecord> _moodRecords;
        private UserCreds _userCreds;

        public static DataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataStore();
                        }
                    }
                }
                return _instance;
            }
        }
    }
    
    
}