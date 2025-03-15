namespace TrackerApp
{
    public class MoodRecord(string mood, string? trigger = null)
    {
        public string Mood { get; } = mood;
        public string? Trigger { get; } = trigger;
    }
    
}