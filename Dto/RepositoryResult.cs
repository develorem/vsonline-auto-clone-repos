using System.Collections.Generic;

namespace AutoCloner.VsOnline.Dto
{
    public class RepositoryResult
    {
        public IEnumerable<Repository> Value { get; set; }
        public int Count { get; set; }
    }
}
