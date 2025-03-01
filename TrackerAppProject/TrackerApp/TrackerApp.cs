using System;


// static string GetUserInput(){
//     Console.WriteLine("Please enter emotion, or 'quit' to end:");
//     string userinput = Console.ReadLine() ?? string.Empty;
//     return userinput;
// }

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
    // TODO
    // - display prompt
    // - get the input
    List<string> emotions = new List<string> { "Happy", "Sad", "Mad", "Indifferent" };
    string prompt = "Enter an emotion, or 'quit' to exit the app: ";
    string userinput = GetUserInput(prompt, emotions, "Happy");
    // - process the input
    still_running = ProcessUserInput(userinput);
    // - record relevant data in datastore ( handle within processUserInput ?)
}
