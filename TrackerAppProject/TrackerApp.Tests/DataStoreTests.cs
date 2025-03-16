using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace TrackerApp.Tests
{
    public class DataStoreTests
    {
        [Fact]
        public void DataStore_Should_Be_Singleton()
        {
            var instance1 = DataStore.Instance;
            var instance2 = DataStore.Instance;
            
            Assert.Same(instance1, instance2);
            Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());
        }
    }
}