using Spectre.Console;

namespace TrackerApp;

public class LoginHandler
{
    public static UserAccount HandleLogin()
    {
        var dataStore = DataStore.Instance;

        if (dataStore.IsFirstLaunch()) return HandleNewUser();

        return HandleReturningUser();
    }

    public static UserAccount HandleNewUser()
    {
        AnsiConsole.MarkupLine("[yellow]First time using the app? Let's set up your account.[/]");

        var username = AnsiConsole.Ask<string>("[green]Enter a username to create an account:[/]");
        // string password = AnsiConsole.Prompt(
        //     new TextPrompt<string>("[green]Create a password:[/]")
        //         .Secret());
        //
        // string passwordHash = HashPassword(password);

        // UserAccount newUser = new() { Username = username, PasswordHash = passwordHash };
        UserAccount newUser = new() { Username = username };

        var dataStore = DataStore.Instance;
        dataStore.SetUserCredentials(newUser);

        AnsiConsole.MarkupLine("Account created successfully!");
        return newUser;
    }

    public static UserAccount HandleReturningUser()
    {
        var dataStore = DataStore.Instance;
        var userAccount = dataStore.GetUserCredentials();
        var storedUsername = userAccount.Username;

        AnsiConsole.MarkupLine($"Welcome back, [bold]{storedUsername}[/]!");

        // bool isAuthenticated = false;
        //
        // while (!isAuthenticated)
        // {
        //     string password;
        //
        //     while (true)
        //     {
        //         password = AnsiConsole.Prompt(
        //             new TextPrompt<string>("[green]Password:[/]")
        //                 .Secret()
        //                 .AllowEmpty());
        //
        //         if (!string.IsNullOrWhiteSpace(password))
        //         {
        //             break;
        //         }
        //
        //         AnsiConsole.MarkupLine("[red]**** Enter a password ****[/]");
        //     }
        //
        //     string passwordHash = HashPassword(password);
        //
        //     if (passwordHash == userAccount.PasswordHash)
        //     {
        //         AnsiConsole.MarkupLine("[green]Login successful![/]");
        //         isAuthenticated = true;
        //     }
        //     else
        //     {
        //         AnsiConsole.MarkupLine("[red]Incorrect password. Please try again.[/]");
        //     }
        // }

        return userAccount;
    }

    private static string HashPassword(string password)
    {
        // Not using crypto at the moment, it's a class project
        // return Convert.ToBase64String(password);
        return password;
    }
}