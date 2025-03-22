namespace TrackerApp
{
    public class AppData
    {
        public UserCreds UserCredentials { get; set; } = new();
        public List<MoodRecord> MoodRecords { get; set; } = new();
    }

    public class UserCreds
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class DataStore
    {
        private static DataStore? _instance;
        private static readonly object _lock = new();
        private readonly string _databaseFilePath;
        private List<MoodRecord> _moodRecords;
        private UserCreds _userCreds;

        private DataStore()
        {
            _databaseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mood_data.json");
            _moodRecords = (List<MoodRecord>) [];
            _userCreds = new UserCreds();
        }

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

        public bool isFirstLaunch()
        {
            return !File.Exists(_databaseFilePath);
        }
    }
    
    
}