using System.Text.Json;

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
        private static readonly object Lock = new();
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
                if (_instance != null)
                {
                    return _instance;
                }

                lock (Lock)
                {
                    _instance ??= new DataStore();
                }

                return _instance;
            }
        }

        private void SaveData()
        {
            AppData appData = new AppData
            {
                UserCredentials = _userCreds, 
                MoodRecords = _moodRecords
            };
            
            string jsonString = JsonSerializer.Serialize(appData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_databaseFilePath, jsonString);
        }
        
        
        public bool IsFirstLaunch()
        {
            return !File.Exists(_databaseFilePath);
        }

        public UserCreds GetUserCredentials()
        {
            return _userCreds;
        }

        public SetUserCredentials(UserCreds userCredentials)
        {
            _userCreds = userCredentials;
            SaveData();
        }
        public List<MoodRecord> GetMoodRecords()
        {
            return _moodRecords;
        }
        
        
    }
    
    
}