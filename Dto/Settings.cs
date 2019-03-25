namespace AutoCloner.VsOnline
{
    public class Settings
    {
        /// <summary>
        /// Username is your email address to login to VS Online
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// This can be your password, or an app token you create. 
        /// </summary>
        public string Password { get; set; }
        public string Organisation { get; set; }
        public string CloneBaseDirectory { get; set; }
        public long? MaxRepoSize { get; set; }
    }
}
