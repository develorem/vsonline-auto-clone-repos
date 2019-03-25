using System.Diagnostics;

namespace AutoCloner.VsOnline
{
    public interface IGitRunner
    {
        string Clone(string url, string fromDirectory);
    }
    public class CommandLineGitRunner : IGitRunner
    {
        public string Clone(string url, string fromDirectory)
        {
            var gitInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = "git.exe", // assumes git is in path
                Arguments = "clone " + url,
                WorkingDirectory = fromDirectory
            };
            var gitProcess = new Process
            {
                StartInfo = gitInfo
            };
            gitProcess.Start();            
            gitProcess.WaitForExit();
            
            var stderr = gitProcess.StandardError.ReadToEnd();
            var stdout = gitProcess.StandardOutput.ReadToEnd();

            gitProcess.Close();
            gitProcess.Dispose();

            return stdout;
        }
    }
}
