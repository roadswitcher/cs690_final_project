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

public class TrackerAppUserInputTests
{
    [Fact]
    public void AskForMood_ShouldReturnUserInput()
    {
        // Arrange
        var console = new TestConsole().EmitAnsiSequences();

        // Simulate Down Arrow + Enter (selects "Sad")
        console.Input.PushText("\x1b[B\n");

        List<string> emotions = new() { "Happy", "Sad", "Mad", "Indifferent", "Quit" };

        var handler = new UserInputHandler(console); // Inject TestConsole here

        // Act
        var result = handler.GetUserInput(emotions);

        // Assert
        Assert.Equal("Sad", result);
    }

    [Fact]
    public void AskForMood_ShouldDefaultToFirstOption_WhenNoNavigation()
    {
        // Arrange
        var console = new TestConsole().EmitAnsiSequences();
        console.Input.PushText("\n"); // Simulate Enter without navigation

        List<string> emotions = new() { "Happy", "Sad", "Mad", "Indifferent", "Quit" };

        var handler = new UserInputHandler(console);

        // Act
        var result = handler.GetUserInput(emotions);

        // Assert
        Assert.Equal("Happy", result); // Default to first option
    }

    [Fact]
    public void ProcessUserInput_ShouldReturnFalse_WhenUserEntersQuit()
    {
        // Act & Assert
        Assert.False(UserInputHandler.ProcessUserInput("Quit"));
    }

    [Fact]
    public void ProcessUserInput_ShouldReturnTrue_WhenUserEntersNonQuitValue()
    {
        // Act & Assert
        Assert.True(UserInputHandler.ProcessUserInput("Continue"));
    }
}