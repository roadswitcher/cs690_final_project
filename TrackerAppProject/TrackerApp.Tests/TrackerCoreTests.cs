using Moq;
using Spectre.Console;
using Spectre.Console.Testing;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Xunit;

public class BasicPipelineDummyTest
{
    [Fact]
    public void DummyTest()
    {
        Assert.True(true); // Placeholder test
    }
}

public class UserInputHandlerTests
{
    [Fact]
    public void Constructor_NullConsole_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new UserInputHandler(null));
    }



    [Theory]
    [InlineData("Happy", true)]
    [InlineData("Sad", true)]
    [InlineData("Report", true)]
    [InlineData("Quit", false)]
    [InlineData("quit", false)]  // Test case sensitivity
    public void ProcessUserInput_ReturnsExpectedResult(string input, bool expectedResult)
    {
        // Act
        var result = UserInputHandler.ProcessUserInput(input);

        // Assert
        Assert.Equal(expectedResult, result);
    }


}