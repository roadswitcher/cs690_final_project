using System.Text.Json;

namespace TrackerApp
{
    public class AppData
    {
        public UserCreds UserCredentials { get; init; } = new();
        public List<MoodRecord> MoodRecords { get; init; } = [];
    }

    public class UserCreds
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public interface IDataStore
    {
        List<MoodRecord> GetMoodRecords();
    }

    public class DataStore : IDataStore
    {
        private static DataStore? _instance;
        private static readonly object Lock = new();
        private readonly string _databaseFilePath;
        private List<MoodRecord> _moodRecords = [];
        private UserCreds _userCredentials = new();

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

        public List<MoodRecord> GetMoodRecords()
        {
            return _moodRecords;
        }

        private void SaveData()
        {
            AppData appData = new() { UserCredentials = _userCredentials, MoodRecords = _moodRecords };

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
                    _userCredentials = appData.UserCredentials;
                    return;
                }
            }

            // If we get to this point, default values
            _moodRecords = [];
            _userCredentials = new UserCreds();
        }

        public bool IsFirstLaunch()
        {
            return !File.Exists(_databaseFilePath);
        }

        public UserCreds GetUserCredentials()
        {
            return _userCredentials;
        }

        public void SetUserCredentials(UserCreds userCredentials)
        {
            _userCredentials = userCredentials;
            SaveData();
        }

        public int GetMoodRecordCount()
        {
            return _moodRecords.Count;
        }

        public void AddMoodRecord(MoodRecord moodRecord)
        {
            _moodRecords.Add(moodRecord);
            SaveData();
        }
    }
}