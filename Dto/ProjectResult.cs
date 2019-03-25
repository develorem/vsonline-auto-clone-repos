using System.Collections.Generic;

namespace AutoCloner.VsOnline.Dto
{
    public class ProjectResult
    {
        public int Count { get; set; }
        public IEnumerable<Project> Value { get; set; }
    }
}
