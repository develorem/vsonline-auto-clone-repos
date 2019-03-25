using System;

namespace AutoCloner.VsOnline.Dto
{
    public class Repository
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public string DefaultBranch { get; set; }

        public long Size { get; set; }

        public string RemoteUrl { get; set; } // use this to clone

        public string SshUrl { get; set; }

    }
}
