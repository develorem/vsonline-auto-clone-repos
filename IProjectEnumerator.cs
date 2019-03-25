using System.Collections.Generic;

namespace AutoCloner.VsOnline
{
    public interface IProjectEnumerator
    {
        IEnumerable<string> GetProjects(string username, string password, string org);
    }

    public class HardCodedProjectEnumerator : IProjectEnumerator
    {
        public IEnumerable<string> GetProjects(string username, string password, string org)
        {
            yield return "OnlineOrdering";
            yield return "Architecture";
        }
    }

    // TODO Implement an Api based proejct enumerator to hit the API and get a list of projects rather than hardcoding
}
