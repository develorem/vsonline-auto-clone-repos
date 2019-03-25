namespace AutoCloner.VsOnline
{
    public static class SettingsProvider
    {
        public static Settings Hardcoded()
        {
            var settings = new Settings
            {
                CloneBaseDirectory = @"C:\src", // TODO Change to your base path where you want to clone everything
                Organisation = "", // TODO Update to your org
                Username = "", // TODO Update to your username for VS Online, usually an email
                Password = "", // TODO Update to your password or token
                MaxRepoSize = 300000000 // TODO What is the largest sized repo you want to get? Delete if you want to get all
            };
            return settings;
        }
        
        public static Settings FromArgs(string[] args)
        {
            // TODO needs validation, support proper switches
            var settings = new Settings
            {
                CloneBaseDirectory = args[0],
                Organisation = args[1],
                Username = args[2],
                Password = args[3],
            };
            return settings;
        }
    }
}
