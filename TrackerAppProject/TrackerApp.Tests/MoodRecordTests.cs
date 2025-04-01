namespace TrackerApp.Tests;

public class MoodRecordTests
{
    [Fact]
    public void MoodRecord_Constructor_SetsProperties()
    {
        var expectedMood = "Happy";
        var expectedTrigger = "There was a dog!";
        DateTime expectedTime = new(2025, 3, 15, 10, 30, 0);

        MoodRecord record = new(expectedMood, expectedTrigger, expectedTime);

        Assert.Equal(expectedMood, record.Mood);
        Assert.Equal(expectedTrigger, record.Trigger);
        Assert.Equal(expectedTime, record.Timestamp);
    }

    [Fact]
    public void MoodRecord_Constructor_IfNoTimeProvided()
    {
        var before = DateTime.UtcNow;

        MoodRecord record = new("Happy", "");
        var after = DateTime.UtcNow;

        Assert.True(record.Timestamp >= before && record.Timestamp <= after);
    }

    [Fact]
    public void MoodRecord_MustHaveMood()
    {
        Assert.Throws<ArgumentNullException>(() => new MoodRecord("", "check for empty string"));
    }
}