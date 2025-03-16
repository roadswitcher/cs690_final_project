namespace TrackerApp.Tests
{
    public class MoodRecordTests
    {
        [Fact]
        public void MoodRecord_Constructor_SetsProperties()
        {

            var expectedMood = "Happy";
            var expectedTrigger = "There was a dog!";
            var expectedTime = new DateTime(2025, 3, 15, 10, 30, 0);
            
            var record = new MoodRecord(expectedMood, expectedTrigger, expectedTime);
            
            Assert.Equal(expectedMood, record.Mood);
            Assert.Equal(expectedTrigger, record.Trigger);
            Assert.Equal(expectedTime, record.Timestamp);
        }

        [Fact]
        public void MoodRecord_Constructor_IfNoTimeProvided()
        {

            var before = DateTime.UtcNow;

            var record = new MoodRecord("Happy", "");
            var after = DateTime.UtcNow;

            Assert.True(record.Timestamp >= before && record.Timestamp <= after);
        }
        
        [Fact]
        public void MoodRecord_MustHaveMood()
        {
            var testRecord = new MoodRecord("", "");
            Assert.Fail();
    }
}