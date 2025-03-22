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
        
        
    }
}