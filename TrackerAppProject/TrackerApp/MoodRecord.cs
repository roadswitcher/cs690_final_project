namespace TrackerApp;

public class MoodRecord
{
    public MoodRecord(string mood, string trigger, DateTime? timestamp = null)
    {
        if (string.IsNullOrEmpty(mood)) throw new ArgumentNullException(nameof(mood), "mood cannot be null or empty");

        Mood = mood;
        Trigger = trigger ?? string.Empty;
        Timestamp = timestamp ?? DateTime.UtcNow;
    }

    public MoodRecord()
    {
        // Default for deserialization
    }

    public string Mood { get; set; } = string.Empty;
    public string Trigger { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}