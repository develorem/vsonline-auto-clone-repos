using System;
using System.Collections.Generic;

namespace vs_api
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

    public class RepositoryResult
    {
        public IEnumerable<Repository> Value { get; set; }
        public int Count { get; set; }
    }
}
