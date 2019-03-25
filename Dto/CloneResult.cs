namespace AutoCloner.VsOnline.Dto
{
    public class CloneResult
    {
        public CloneResult(string project)
        {
            Project = project;
        }

        public string Name { get; set; }
        public CloneStatus Status { get; set; }
        public string ClonePath { get; set; }
        public string Project { get; }
    }

    public enum CloneStatus
    {
        Success,
        Exception,
        FolderExists,
        ExceededMaxRepositorySizeCheck
    }
}
