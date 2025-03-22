using Spectre.Console;

namespace TrackerApp
{
    public class LoginHandler
    {
        private readonly DataStore _dataStore;
        private readonly IAnsiConsole _console;

        public LoginHandler(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _dataStore = DataStore.Instance;
        }

        public bool HandleLogin()
        {
            if (_dataStore.isFirstLaunch())
            {
                return ProcessFirstLogin();
            }
            else
            {
                return ProcessReturnLogin();
            }
        }

        private bool ProcessFirstLogin()
        {
            _console.WriteLine("Welcome!  Let's set up an account.");
            string username = _console.Ask<string>("Enter your [yellow]username[/]: ");
            if (string.IsNullOrWhiteSpace(username))
            {
                _console.WriteLine("[red]Invalid username, cannot be null or empty.[/]");
                return false;
            }
            string password = _console.Ask<string>("Enter your [yellow]password[/]: ");  
        }
        
    }
}