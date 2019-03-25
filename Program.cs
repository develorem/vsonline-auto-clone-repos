using Microsoft.Extensions.DependencyInjection;
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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Multi-project clone engine.");
            Console.WriteLine();

            var settings = SettingsProvider.Hardcoded();

            var engine = RegisterServicesAndGetEngine();
            var results = engine.Run(settings);

            var changes = results.Where(x => x.Status != CloneStatus.FolderExists).ToArray();
            LogNewResults(settings.CloneBaseDirectory, changes);

            Console.WriteLine();
            Console.WriteLine("Multi-project clone engine completed.");
            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine();
        }

        private static void LogNewResults(string path, IEnumerable<CloneResult> results)
        {
            var now = DateTime.Now.ToString("yy-mm-dd-hh-MM-ss");
            var filename = Path.Combine(path, $"RepoChanges-{now}.json");

            var json = JsonConvert.SerializeObject(results);
            File.WriteAllText(filename, json);
        }

        private static Engine RegisterServicesAndGetEngine()
        {
            var services = new ServiceCollection()
                .AddSingleton<Engine>()
                .AddSingleton<IGitRunner, CommandLineGitRunner>()
                .AddSingleton<ICloner, GitCloner>()
                .AddSingleton<IVsOnlineApiClient, VsOnlineApiClient>()
                .AddSingleton<IRepositoryEnumerator, VsOnlineRepositoryEnumerator>()
                .AddSingleton<IProjectEnumerator, HardCodedProjectEnumerator>()
                .AddLogging(c => c.AddConsole())
                ;

            var provider = services.BuildServiceProvider();

            return provider.GetService<Engine>();
        }

    }
}
