using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoCloner.VsOnline.Dto;

namespace AutoCloner.VsOnline
{
    public class Engine
    {
        private readonly IProjectEnumerator projectEnumerator;
        private readonly ICloner cloner;
        private readonly IRepositoryEnumerator repositoryEnumerator;
        private readonly ILogger<Engine> logger;

        public Engine(IProjectEnumerator projectEnumerator, ICloner cloner, IRepositoryEnumerator repositoryEnumerator, ILogger<Engine> logger)
        {
            this.projectEnumerator = projectEnumerator;
            this.cloner = cloner;
            this.repositoryEnumerator = repositoryEnumerator;
            this.logger = logger;
        }

        public IEnumerable<CloneResult> Run(Settings settings)
        {
            var results = new ConcurrentBag<CloneResult>();

            if (!Directory.Exists(settings.CloneBaseDirectory))
            {
                throw new ArgumentException("Must provide CloneBaseDirectory that actually exists");
            }

            var projects = projectEnumerator.GetProjects(settings.Username, settings.Password, settings.Organisation);

            foreach (var project in projects)
            {
                var projectPath = Path.Combine(settings.CloneBaseDirectory, project);
                EnsurePath(projectPath);

                var projectRepositories =
                    repositoryEnumerator.GetRepositories(settings.Username, settings.Password, settings.Organisation, project);

                var bigRepos = projectRepositories.Where(x => x.Size > 100000).ToArray();

                DumpRepositoriesAsJson(projectPath, projectRepositories);

                Parallel.ForEach(projectRepositories, repo => {
                    var result = CloneRepository(project, projectPath, repo, settings.MaxRepoSize);
                    results.Add(result);
                });
            }
            return results;
        }

        private void DumpRepositoriesAsJson(string projectPath, IEnumerable<Repository> projectRepositories)
        {
            var json = JsonConvert.SerializeObject(projectRepositories);
            var filename = Path.Combine(projectPath, $"ProjectRepositoryList.json");
            if (File.Exists(filename)) File.Delete(filename);
            File.WriteAllText(filename, json);
        }

        private CloneResult CloneRepository(string projectName, string baseClonePath, Repository repo, long? maxRepoSize)
        {
            var result = cloner.CloneIfNotExists(projectName, baseClonePath, repo, maxRepoSize);
           
            switch (result.Status)
            {
                case Dto.CloneStatus.Success:
                    logger.LogInformation($"Cloning: {projectName}/{repo.Name} - Success");
                    break;
                case Dto.CloneStatus.Exception:
                    logger.LogError($"Cloning: {projectName}/{repo.Name} - Failed, exception");
                    break;
                case Dto.CloneStatus.FolderExists:
                    logger.LogWarning($"Cloning: {projectName}/{repo.Name} - Folder already exists, ignored");
                    break;
                case Dto.CloneStatus.ExceededMaxRepositorySizeCheck:
                    logger.LogWarning($"Cloning: {projectName}/{repo.Name} - Did not clone due to repository size");
                    break;
            }

            return result;
        }

        private void EnsurePath(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
