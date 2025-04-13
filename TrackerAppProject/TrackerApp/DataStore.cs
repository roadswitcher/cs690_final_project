using System.Text.Json;
using TrackerApp.Utilities;

namespace TrackerApp;

public class AppData
{
    public UserAccount UserCredentials { get; init; } = new();
    public List<MoodRecord> MoodRecords { get; init; } = [];
}

public class UserAccount
{
    public string Username { get; set; } = string.Empty;
    // public string PasswordHash { get; set; } = string.Empty;
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
    private UserAccount _userCredentials = new();

    private DataStore()
    {
        _databaseFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mood_data.json");
        LoadData();
    }

    public static DataStore Instance
    {
        get
        {
            if (_instance != null) return _instance;

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

        var jsonString = JsonSerializer.Serialize(appData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_databaseFilePath, jsonString);
    }

    private void LoadData()
    {
        if (File.Exists(_databaseFilePath))
        {
            var jsonString = File.ReadAllText(_databaseFilePath);
            var appData = JsonSerializer.Deserialize<AppData>(jsonString);


            if (appData != null)
            {
                _moodRecords = appData.MoodRecords;
                _userCredentials = appData.UserCredentials;
                return;
            }
        }

        // If we get to this point, default values
        _moodRecords = [];
        _userCredentials = new UserAccount();
    }

    public bool HasDemoTrigger()
    {
        return _moodRecords.Any(record =>
            record.Trigger.StartsWith("(demo)", StringComparison.OrdinalIgnoreCase));
    }

    public void AddTheDemoData()
    {
        var demoRecords = DemoMoodGenerator.GenerateMoodUpdates();

        if (HasDemoTrigger()) return;
        foreach (var record in demoRecords) AddMoodRecord(record);
        SaveData();
    }

    public void DeleteDemoData()
    {
        _moodRecords.RemoveAll(record =>
            record.Trigger.StartsWith("(demo) ", StringComparison.OrdinalIgnoreCase));
        SaveData();
    }

    public bool IsFirstLaunch()
    {
        return !File.Exists(_databaseFilePath);
    }

    public UserAccount GetUserCredentials()
    {
        return _userCredentials;
    }

    public void SetUserCredentials(UserAccount userCredentials)
    {
        _userCredentials = userCredentials;
        SaveData();
    }

    public int GetMoodRecordCount()
    {
        return _moodRecords.Count;
    }

    public bool ContainsRecords()
    {
        return _moodRecords.Count > 0;
    }

    public MoodRecord GetLastMoodRecord()
    {
        return _moodRecords[^1];
    }

    public bool RemoveLastMoodRecord()
    {
        if (_moodRecords.Count == 0) return false;

        _moodRecords.RemoveAt(_moodRecords.Count - 1);
        SaveData();
        TrackerUtils.DebugMessage(" *** Removed last mood record");

        return true;
    }

    public void AddMoodRecord(MoodRecord moodRecord)
    {
        _moodRecords.Add(moodRecord);
        SaveData();
        TrackerUtils.DebugMessage($" *** Mood Record Update: {moodRecord.Mood},  {moodRecord.Trigger}");
        TrackerUtils.DebugMessage($" *** New Mood Record Count: {GetMoodRecordCount()}");
    }
}