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
    [Fact]
    public void AskForMood_ShouldReturnUserInput_WhenSimulatingKeyPresses()
    {
        // Arrange: Create a TestConsole instance for simulating user input.
        var console = new TestConsole().EmitAnsiSequences();

        // \x1b[B = Down Arrow, \n = Enter key
        console.Input.PushText("\x1b[B\n");

        List<string> emotions = new() { "Happy", "Sad", "Mad", "Indifferent", "Quit" };

        var handler = new UserInputHandler(console);

        // Act: Get the user's input (simulated through the TestConsole)
        var result = handler.GetUserInput(emotions);

        // Assert: Verify that the selected mood is "Sad"
        Assert.Equal("Sad", result);  // We simulate Down Arrow to select "Sad"
    }

}