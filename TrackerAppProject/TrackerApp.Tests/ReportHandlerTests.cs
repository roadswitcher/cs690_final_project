using Moq;

namespace TrackerApp.Tests;

public class ReportHandlerTests
{
    private readonly Mock<IDataStore> _mockDataStore;
    private readonly ReportHandler _reportHandler;
    private readonly List<MoodRecord> _testData;
    private readonly DateTime _today;

    public ReportHandlerTests()
    {
        _today = DateTime.UtcNow.Date;
        _mockDataStore = new Mock<IDataStore>();
        _reportHandler = new ReportHandler(_mockDataStore.Object);

        // Intent is to produce a Daily report, and a Last Week report
        _testData = new List<MoodRecord>
        {
            new("Happy", "There was a dog!", _today.AddHours(9)),
            new("Stressed", "Forgot a CS690 deadline", _today.AddHours(14)),
            new("Relaxed", "I had cake", _today.AddHours(19)),

            // Yesterday's records
            new("Tired", "Poor sleep, stayed up too late on CS690", _today.AddDays(-1).AddHours(8)),
            new("Focused", "fixing git errors", _today.AddDays(-1).AddHours(13)),

            // Records from 3 days ago
            new("Anxious", "Watching basketball", _today.AddDays(-3).AddHours(10)),
            new("Proud", "Completed feature for CS690 project", _today.AddDays(-3).AddHours(16)),

            // Records from 6 days ago
            new("Excited", "I made good picks in my basketball bracket",
                _today.AddDays(-6).AddHours(11)),

            // Records from 8 days ago (outside of weekly report)
            new("Frustrated", "running out of ideas for test cases", _today.AddDays(-8).AddHours(18)),

            // Records from 2 weeks ago
            new("Calm", "", _today.AddDays(-14).AddHours(7)),

            // Records from 3 weeks ago
            new("Wistful", "oh joy", _today.AddDays(-21).AddHours(7)),

            // Records from 6 weeks ago
            new("Wistful", "this was so long ago", _today.AddDays(-43).AddHours(1))
        };

        _mockDataStore.Setup(ds => ds.GetMoodRecords()).Returns(_testData);
    }

    [Fact]
    public void GetDailyReport_ReturnsCorrectNumberOfRecords()
    {
        var date = _today;

        var report = _reportHandler.GetDailyReport(date);

        Assert.Equal(3, report.TotalRecords);
        Assert.Equal(date, report.Date);
    }

    [Fact]
    public void GetWeeklyReport_ReturnsCorrectNumberOfRecords()
    {
        var date = _today;
        var report = _reportHandler.GetWeeklyReport(date);

        Assert.Equal(8, report.TotalRecords);
    }
}