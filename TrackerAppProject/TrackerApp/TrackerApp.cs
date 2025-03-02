using System;

string GetUserInput(string prompt, List<string> options, string defaultOption)
{
    if (!options.Contains(defaultOption))
    {
        throw new ArgumentException("ERROR: Default option not provided.");
    }

    Console.Write(prompt);
    Console.Write($"[{defaultOption}]");

    string input = Console.ReadLine() ?? String.Empty;

    return (string.IsNullOrEmpty(input)) ? defaultOption : input;
}


bool ProcessUserInput(string userinput)
{
    Console.WriteLine($"User entered: {userinput}.");

    if (String.Equals(userinput, "quit", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }
    else
    {
        return true;
    }
}
bool still_running = true;

while (still_running)
{

    List<string> emotions = new List<string> { "Happy", "Sad", "Mad", "Indifferent" };
    string prompt = "Enter an emotion, or 'quit' to exit the app: ";
    string userinput = GetUserInput(prompt, emotions, "Happy");

    still_running = ProcessUserInput(userinput);
}
