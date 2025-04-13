using System.Text.Json;

namespace TrackerApp;

public class AppData
{
    public UserAccount UserCredentials { get; init; } = new();
    public List<MoodRecord> MoodRecords { get; init; } = [];
}

public class UserAccount
{
    public string Username { get; init; } = string.Empty;
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
    private static readonly Random Random = new();

    private static readonly string[] Triggers = new string[]
    {
        "Just got an A on my assignment",
        "Had a great conversation with a friend",
        "Struggling with a difficult homework problem",
        "Looking forward to relaxing",
        "Feeling overwhelmed with schoolwork",
        "Just got pizza!",
        "Had a frustrating conversation with a professor",
        "Excited for my afternoon class",
        "Feeling anxious about an upcoming exam",
        "Just finished a long project",
        "Nice bike ride!",
        "Feeling stressed about deadlines",
        "Just got a good grade on a quiz",
        "Had a nice walk outside",
        "Feeling tired and need a nap"
    };

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
        var demoRecords = GenerateMoodUpdates();

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


    public static List<MoodRecord> GenerateMoodUpdates(int numberOfDays = 45)
    {
        // Aim is to do the following:
        // - Generate N days of 'demo' updates going back from current local time when invoked
        // - Add (demo) into the trigger info for removal?  Or just an admin option to 'clear all data'?

        var moods = TrackerUtils.MoodColors.Keys.ToList();
        var moodRecords = new List<MoodRecord>();

        var currentDate = DateTime.Now;
        for (var i = 0; i < numberOfDays; i++)
        {
            var date = currentDate.AddDays(-i);
            var updatesForDay = Random.Next(2, 7); // Random number of updates between 2 and 6 per day

            for (var j = 0; j < updatesForDay; j++)
            {
                var mood = moods[Random.Next(moods.Count)];
                var trigger = "(demo) "; // make deleting demo data easier
                trigger = trigger + (Random.NextDouble() < 0.5 ? Triggers[Random.Next(Triggers.Length)] : null);
                var hour = Random.Next(7, 24); // Confine updates to likely waking hours
                var minute = Random.Next(60);
                var timestamp = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);

                moodRecords.Add(new MoodRecord
                {
                    Mood = mood,
                    Trigger = trigger,
                    Timestamp = timestamp
                });
            }
        }

        return moodRecords;
    }
}