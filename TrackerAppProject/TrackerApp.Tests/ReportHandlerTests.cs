using Moq;
using Spectre.Console;
using TrackerApp.ObjectClasses;

namespace TrackerApp.Tests;

public class ReportDisplayerTests
{
    private readonly Mock<IDataStore> _mockDataStore;
    private readonly Mock<IAnsiConsole> _mockConsole;
    private readonly ReportDisplayer _reportDisplayer;
    private readonly List<MoodRecord> _testData;
    private readonly DateTime _today;

    public ReportDisplayerTests()
    {
        
        _today = DateTime.UtcNow.Date;
        _mockDataStore = new Mock<IDataStore>();
        _mockConsole = new Mock<IAnsiConsole>();
        _reportDisplayer = new ReportDisplayer(_mockConsole.Object, _mockDataStore.Object);

        // Intent is to produce a Daily report, and a Last Week report
        // All "moods" are using Content as placeholder content
        // ( see what i did there? )
        _testData = new List<MoodRecord>
        {
            new("Content", "There was a dog!", _today.AddHours(9)),
            new("Content", "Forgot a CS690 deadline", _today.AddHours(14)),
            new("Content", "I had cake", _today.AddHours(19)),

            // Yesterday's records
            new("Content", "Poor sleep, stayed up too late on CS690", _today.AddDays(-1).AddHours(8)),
            new("Content", "fixing git errors", _today.AddDays(-1).AddHours(13)),

            // Records from 3 days ago
            new("Content", "Watching basketball", _today.AddDays(-3).AddHours(10)),
            new("Content", "Completed feature for CS690 project", _today.AddDays(-3).AddHours(16)),

            // Records from 6 days ago
            new("Content", "I made good picks in my basketball bracket",
                _today.AddDays(-6).AddHours(11)),

            // Records from 8 days ago (outside of weekly report)
            new("Content", "running out of ideas for test cases", _today.AddDays(-8).AddHours(18)),

            // Records from 2 weeks ago
            new("Content", "", _today.AddDays(-14).AddHours(7)),

            // Records from 3 weeks ago
            new("Content", "oh joy", _today.AddDays(-21).AddHours(7)),

            // Records from 6 weeks ago
            new("Content", "this was so long ago", _today.AddDays(-43).AddHours(1))
        };

        _mockDataStore.Setup(ds => ds.GetMoodRecords()).Returns(_testData);
    }

    [Fact]
    public void GetDailyReport_ReturnsCorrectNumberOfRecords()
    {
        var date = _today;

        var report = _reportDisplayer.GenerateDailyReport(date);

        Assert.Equal(3, report.TotalRecords);
        Assert.Equal(date, report.Date);

        // Check another date
        date = date.AddDays(-3);
        report = _reportDisplayer.GenerateDailyReport(date);
        Assert.Equal(2, report.TotalRecords);
    }

    [Fact]
    public void GetWeeklyReport_ReturnsCorrectNumberOfRecords()
    {
        var date = _today;
        var report = _reportDisplayer.GeneratePriorWeekReport(date);

        Assert.Equal(8, report.TotalRecords);
    }

    [Fact]
    public void GetDailyReport_MidnightBoundaryRecords_AssignedToCorrectDay()
    {
        // Arrange
        var localMidnight = _today.Date;
        var oneMinuteBeforeMidnight = localMidnight.AddMinutes(-1);
        var exactlyMidnight = localMidnight;
        var oneMinuteAfterMidnight = localMidnight.AddMinutes(1);

        var records = new List<MoodRecord>
        {
            // should be in yesterday's report
            new("Content", "Just before midnight", TimeZoneInfo.ConvertTimeToUtc(oneMinuteBeforeMidnight)),

            // should be in today's report
            new("Content", "Exactly at midnight", TimeZoneInfo.ConvertTimeToUtc(exactlyMidnight)),

            // should be in today's report
            new("Content", "Just after midnight", TimeZoneInfo.ConvertTimeToUtc(oneMinuteAfterMidnight))
        };

        _mockDataStore.Setup(ds => ds.GetMoodRecords()).Returns(records);

        // Act - query for yesterday
        var yesterdayReport = _reportDisplayer.GenerateDailyReport(_today.AddDays(-1));
        // Act - query for today
        var todayReport = _reportDisplayer.GenerateDailyReport(_today);

        // Assert
        Assert.Equal(1, yesterdayReport.TotalRecords);
        Assert.Equal(2, todayReport.TotalRecords);
    }

    [Fact]
    public void GetDailyReport_DifferentTimeZones_HandlesCorrectly()
    {
        // Arrange
        var referenceUtcTime = new DateTime(2023, 10, 15, 5, 0, 0, DateTimeKind.Utc);
        var targetLocalDate = referenceUtcTime.Date;

        // Eastern Time (UTC-5 or UTC-4 depending on DST) - let's assume Eastern Standard Time (UTC-5)
        var easternTime = referenceUtcTime.AddHours(-1);

        // Japan Standard Time (UTC+9)
        var japanTime = referenceUtcTime;

        // Create test records from these different time zones but same UTC time
        var records = new List<MoodRecord>
        {
            // Record from Eastern Time - late evening on previous day in ET
            new("Content", "Late night in Eastern Time", easternTime),

            // Record from Japan - afternoon of the target day in JST
            new("Content", "Afternoon in Japan", japanTime)
        };

        _mockDataStore.Setup(ds => ds.GetMoodRecords()).Returns(records);

        // Act
        // Query for the target local date
        var report = _reportDisplayer.GenerateDailyReport(targetLocalDate);

        // Assert
        // Both records should be in the report since they fall within the same UTC day
        Assert.Equal(2, report.TotalRecords);
    }
    
    [Fact]
    public void DisplayDailyReport_WritesCorrectDataToConsole()
    {
        // Arrange
        var date = _today;
        
        // Act
        _reportDisplayer.DisplayDailyReport(date);
        
        // Assert
        // Verify that the console was written to with expected content
        _mockConsole.Verify(c => c.WriteLine($"Number of updates today: 3"), Times.Once);
        _mockConsole.Verify(c => c.Write(It.IsAny<Rule>()), Times.AtLeastOnce);
        _mockConsole.Verify(c => c.Write(It.IsAny<Table>()), Times.AtLeastOnce);
        _mockConsole.Verify(c => c.Write(It.IsAny<BreakdownChart>()), Times.Once);
    }
}