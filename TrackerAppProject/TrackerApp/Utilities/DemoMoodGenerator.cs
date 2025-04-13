namespace TrackerApp.Utilities;

public class DemoMoodGenerator
{
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