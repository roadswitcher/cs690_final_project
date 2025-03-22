using Spectre.Console;

namespace TrackerApp
{
    public class LoginHandler
    {
        public static UserCreds HandleLogin(IAnsiConsole? console = null)
        {
            DataStore dataStore = DataStore.Instance;
            console ??= AnsiConsole.Console;

            if (dataStore.IsFirstLaunch())
            {
                return HandleNewUser(console);
            }
            else
            {
                return HandleReturningUser(console);
            }
        }

        private static UserCreds HandleNewUser(IAnsiConsole console)
        {
            console.MarkupLine("[yellow]First time using the app? Let's set up an account![/]");
            string username = console.Ask<string>("Enter a username: ");
            string password = console.Ask<string>("Enter a password: ");
            string passwordHash = HashPassword(password);

            UserCreds newUser = new UserCreds { Username = username, PasswordHash = passwordHash };
            AppData appData = new AppData { UserCredentials = newUser, MoodRecords = new List<MoodRecord>() };
            
        }
        
        private static UserCreds HandleReturningUser() { }

        private static string HashPassword(string password)
        {
            // Not using crypto at the moment, it's a class project
            // TODO: time permitting, checkout out System.Security to see
            //       what'll work
            return Convert.ToBase64String(password);
        }
        
        
        
    }
}