using System.Collections.Generic;
using AutoCloner.VsOnline.Dto;

namespace AutoCloner.VsOnline
{
    public interface IRepositoryEnumerator
    {
        IEnumerable<Repository> GetRepositories(string username, string password, string org, string project);
    }

    public class VsOnlineRepositoryEnumerator : IRepositoryEnumerator
    {
        private readonly IVsOnlineApiClient apiClient;

        public VsOnlineRepositoryEnumerator(IVsOnlineApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public IEnumerable<Repository> GetRepositories(string username, string password, string org, string project)
        {
            apiClient.SetBasicCredentials(username, password);
            var result = apiClient.GetApi<RepositoryResult>(org, project, @"git/repositories");
            return result.Value;
        }
    }
}
