using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using AutoCloner.VsOnline.Dto;

namespace AutoCloner.VsOnline
{
    public interface ICloner
    {
        CloneResult CloneIfNotExists(string projectName, string baseDir, Repository repo, long? maxRepoSizeToClone);
    }

    public class GitCloner : ICloner
    {
        private readonly ILogger<GitCloner> logger;
        private readonly IGitRunner gitRunner;

        public GitCloner(ILogger<GitCloner> logger, IGitRunner gitRunner)
        {
            this.logger = logger;
            this.gitRunner = gitRunner;
        }

        public CloneResult CloneIfNotExists(string projectName, string baseDir, Repository repo, long? maxRepoSizeToClone)
        {
            var result = new CloneResult(projectName) { Name = repo.Name, Status = CloneStatus.Exception };
            if (maxRepoSizeToClone.HasValue && repo.Size > maxRepoSizeToClone.Value)
            {
                result.Status = CloneStatus.ExceededMaxRepositorySizeCheck;
                return result;
            }

            try
            {
                result.ClonePath = Path.Combine(baseDir, repo.Name); 
                if (!Directory.Exists(result.ClonePath))
                {
                    gitRunner.Clone(repo.RemoteUrl, baseDir);
                    result.Status = CloneStatus.Success;
                }
                else
                {
                    result.Status = CloneStatus.FolderExists;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failure while cloning {repo.Name} - {repo.RemoteUrl}");
                result.Status = CloneStatus.Exception;
            }

            return result;
        }
    }
}
