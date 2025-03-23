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
            LoadData();
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

        private void LoadData()
        {
            if (File.Exists(_databaseFilePath))
            {
                string jsonString = File.ReadAllText(_databaseFilePath);
                AppData? appData = JsonSerializer.Deserialize<AppData>(jsonString);

                if (appData != null)
                {
                    _moodRecords = appData.MoodRecords;
                    _userCreds = appData.UserCredentials;
                    return;
                }
            }
            
            // If we get to this point, default values
            _moodRecords = new List<MoodRecord>();
            _userCreds = new UserCreds();
        }
        
        public bool IsFirstLaunch()
        {
            return !File.Exists(_databaseFilePath);
        }

        public UserCreds GetUserCredentials()
        {
            return _userCreds;
        }

        public void SetUserCredentials(UserCreds userCredentials)
        {
            _userCreds = userCredentials;
            SaveData();
        }
        public List<MoodRecord> GetMoodRecords()
        {
            return _moodRecords;
        }

        public void AddMoodRecord(MoodRecord moodRecord)
        {
            _moodRecords.Add(moodRecord);
            SaveData();
        }
        
    }
    
    
}