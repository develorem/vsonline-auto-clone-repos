namespace AutoCloner.VsOnline
{
    public class Settings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Organisation { get; set; }
        public string CloneBaseDirectory { get; set; }
        public long? MaxRepoSize { get; set; }
    }
}
