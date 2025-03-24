using Spectre.Console;

namespace TrackerApp
{
    public class LoginHandler
    {
        public static UserCreds HandleLogin()
        {
            DataStore dataStore = DataStore.Instance;

            if (dataStore.IsFirstLaunch())
            {
                return HandleNewUser();
            }

            return HandleReturningUser();
        }

        public static UserCreds HandleNewUser()
        {
            AnsiConsole.MarkupLine("[yellow]First time using the app? Let's set up your account.[/]");

            string username = AnsiConsole.Ask<string>("[green]Enter a username:[/]");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Create a password:[/]")
                    .Secret());

            string passwordHash = HashPassword(password);

            UserCreds newUser = new() { Username = username, PasswordHash = passwordHash };

            DataStore dataStore = DataStore.Instance;
            dataStore.SetUserCredentials(newUser);

            AnsiConsole.MarkupLine("Account created successfully!");
            return newUser;
        }

        public static UserCreds HandleReturningUser()
        {
            DataStore dataStore = DataStore.Instance;
            UserCreds userCreds = dataStore.GetUserCredentials();
            string storedUsername = userCreds.Username;

            AnsiConsole.MarkupLine($"Welcome back, [bold]{storedUsername}[/]!");

            bool isAuthenticated = false;

            while (!isAuthenticated)
            {
                string password;

                while (true)
                {
                    password = AnsiConsole.Prompt(
                        new TextPrompt<string>("[green]Password:[/]")
                            .Secret()
                            .AllowEmpty());

                    if (!string.IsNullOrWhiteSpace(password))
                        break;

                    AnsiConsole.MarkupLine("[red]**** Enter a password ****[/]");
                }

                string passwordHash = HashPassword(password);

                if (passwordHash == userCreds.PasswordHash)
                {
                    AnsiConsole.MarkupLine("[green]Login successful![/]");
                    isAuthenticated = true;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Incorrect password. Please try again.[/]");
                }
            }

            return userCreds;
        }

        private static string HashPassword(string password)
        {
            // Not using crypto at the moment, it's a class project
            // TODO: time permitting, checkout out System.Security to see
            //       what'll work
            // return Convert.ToBase64String(password);
            return password;
        }
    }
}