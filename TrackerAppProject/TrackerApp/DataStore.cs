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
        public string Password { get; set; } = string.Empty;
    }

    public class DataStore
    {
        private static DataStore? _instance;
        private static readonly object _lock = new();
        private string _databaseFilePath;
        private List<MoodRecord> _moodRecords;
        private UserCreds _userCreds;

        private DataStore()
        {
            _databaseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mooddata.json");
            _moodRecords = new List<MoodRecord>();
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

        private void SaveData()
        {
            try
            {
                AppData data = new AppData { UserCredentials = _userCreds, MoodRecords = _moodRecords };

                string jsonData = JsonSerializer.Serialize(data);

                File.WriteAllText(_databaseFilePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ***** ERROR SAVING DATA *****");
            }
        }

        public void LoadData()
        {
            if (File.Exists(_databaseFilePath))
            {
                try
                {
                    string jsonData = File.ReadAllText(_databaseFilePath);
                    AppData data = JsonSerializer.Deserialize<AppData>(jsonData);
                    if (data != null)
                    {
                        _moodRecords = data.MoodRecords;
                        _userCreds = data.UserCredentials;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" ***** ERROR LOADING DATA *****");
                }
            }
        }
        public void SetUserCredentials(string username, string password)
        {
            _userCreds.Username = username;
            _userCreds.Password = password;
            SaveData();
        }

        public bool CheckPassword(string password)
        {
            return !string.IsNullOrEmpty(_userCreds.Password) && _userCreds.Password == password;
        }

        public bool HasAUser()
        {
            return !string.IsNullOrEmpty(_userCreds.Username) && !string.IsNullOrEmpty(_userCreds.Password);
        }
        public bool isFirstLaunch()
        {
            return !File.Exists(_databaseFilePath);
        }

        public UserCreds getUserCredentials()
        {
            return _userCreds;
        }

        public List<MoodRecord> getMoodRecords()
        {
            return _moodRecords;
        }
    }
    
    
}