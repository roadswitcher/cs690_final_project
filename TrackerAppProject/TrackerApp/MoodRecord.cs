namespace TrackerApp
{
    public class MoodRecord
    {
        public MoodRecord(string mood, string trigger, DateTime? timestamp = null)
        {
            if (string.IsNullOrEmpty(mood))
            {
                throw new ArgumentNullException(nameof(mood), "mood cannot be null or empty");
            }

            Mood = mood;
            Trigger = trigger != null ? trigger : string.Empty;
            Timestamp = timestamp != null ? timestamp.Value : DateTime.UtcNow;
        }

        public string Mood { get; }
        public string Trigger { get; }
        public DateTime Timestamp { get; }
    }
}