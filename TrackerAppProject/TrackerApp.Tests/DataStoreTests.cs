namespace TrackerApp.Tests
{
    public class DataStoreTests
    {
        [Fact]
        public void DataStore_Should_Be_Singleton()
        {
            var instance1 = new DataStore();
            var instance2 = new DataStore();
            
            Assert.Same(instance1, instance2);
            Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());
        }
    }
}