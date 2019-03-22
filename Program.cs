using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace vs_api
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseDir = @"C:\clonetest"; // you will need to create this folder
            var myOrg = "todo";
            var myProject = "todo";

            var settings = new Settings();
            var git = new GitRunner();
            var apiClient = new VsOnlineApiClient(settings);
            Console.Write("Pulling repository list from VS online...");
            var result = apiClient.GetApi<RepositoryResult>(myOrg, myProject, @"git/repositories");
            Console.WriteLine($"Completed. There are {result.Value.Count()} repositories retrieved.");

            var preexisting = result.Value.Where(x => FolderExistsForRepo(baseDir, x)).Count();
            var needToClone = result.Value.Where(x => FolderExistsForRepo(baseDir, x) == false);
            Console.WriteLine($"Folders exist for {preexisting} repositories. Will clone {needToClone.Count()} repositories");

            var bag = new ConcurrentBag<CloneResult>();

            Console.WriteLine("Cloning:");
            Parallel.ForEach(needToClone, r => CloneIfNotExists(git, baseDir, r, bag));
            Console.WriteLine();
            Console.WriteLine("Cloning completed. Exporting...");
            var filename  = DumpResults(baseDir, bag);
            Console.WriteLine("Export complete. Results at: " + filename);
            Console.ReadKey();
        }

        private static bool FolderExistsForRepo(string baseDir, Repository repo)
        {
            var dir = Path.Combine(baseDir, repo.Name);
            return Directory.Exists(dir);
        }

        private static string DumpResults(string path, ConcurrentBag<CloneResult> results)
        {
            var json = JsonConvert.SerializeObject(results.ToArray());
            var filename = Path.Combine(path, DateTime.Now.ToString("yyyy-mm-dd-hh-MM-ss") + ".json");
            File.WriteAllText(filename, json);
            return filename;
        }

        private static void CloneIfNotExists(GitRunner git, string baseDir, Repository repo, ConcurrentBag<CloneResult> bag)
        {
            var result = new CloneResult { Name = repo.Name,  ClonedSuccessfully = false };

            try
            {
                var newPath = Path.Combine(baseDir, repo.Name);
                if (!Directory.Exists(newPath))
                {

                    git.Clone(repo.RemoteUrl, baseDir);
                }
                result.ClonedSuccessfully = true;
            }
            catch (Exception ex)
            {
                result.ClonedSuccessfully = false;
            }

            bag.Add(result);

            Console.Write($".");
        }
    }

    public class CloneResult
    {
        public string Name { get; set; }
        public bool ClonedSuccessfully { get; set; }
    }
}
