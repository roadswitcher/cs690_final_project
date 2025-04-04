using System.Reflection;

namespace TrackerApp.Tests;

public class DataStoreTests
{
    private readonly DataStore _dataStore;
    private readonly string? _testDataPath;

    public DataStoreTests()
    {
        // Setup - runs before each test
        _dataStore = DataStore.Instance;

        // get the data file path with reflection
        var fieldInfo = typeof(DataStore).GetField("_dataFilePath",
            BindingFlags.NonPublic | BindingFlags.Instance);
        var value = fieldInfo?.GetValue(_dataStore);
        _testDataPath = value != null ? (string)value : string.Empty;

        // Delete the test file if it exists
        if (File.Exists(_testDataPath)) File.Delete(_testDataPath);
    }

    [Fact]
    public void DataStore_Should_Be_Singleton()
    {
        var instance1 = DataStore.Instance;
        var instance2 = DataStore.Instance;

        Assert.Same(instance1, instance2);
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());
    }

    [Fact]
    public void IsNewLaunch_ShouldBeTrue_If_No_Data_Store_Exists()
    {
        Assert.True(_dataStore.IsFirstLaunch());
    }
}