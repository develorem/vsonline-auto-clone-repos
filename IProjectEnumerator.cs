using AutoCloner.VsOnline.Dto;
using System.Collections.Generic;
using System.Linq;

namespace AutoCloner.VsOnline
{
    public interface IProjectEnumerator
    {
        IEnumerable<string> GetProjects(string username, string password, string org);
    }
    
    public class VsOnlineApiProjectEnumerator : IProjectEnumerator
    {
        private readonly IVsOnlineApiClient apiClient;

        public VsOnlineApiProjectEnumerator(IVsOnlineApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public IEnumerable<string> GetProjects(string username, string password, string org)
        {
            apiClient.SetBasicCredentials(username, password);
            var projectResult = apiClient.GetApi<ProjectResult>(org, "projects");
            return projectResult.Value.Select(x => x.Name).ToArray();
        }
    }
}
