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
            else
            {
                return HandleReturningUser();
            }
        }

        private static UserCreds HandleNewUser() { }
        
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