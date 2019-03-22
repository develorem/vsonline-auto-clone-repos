using System.Diagnostics;

namespace vs_api
{
    public class GitRunner
    {
        public string Clone(string url, string fromDirectory)
        {
            var gitInfo = new ProcessStartInfo();
            gitInfo.CreateNoWindow = true;
            gitInfo.RedirectStandardError = true;
            gitInfo.RedirectStandardOutput = true;
            gitInfo.UseShellExecute = false;
            gitInfo.FileName = "git.exe"; // assumes git is in path

            var gitProcess = new Process();
            gitInfo.Arguments = "clone " + url;
            gitInfo.WorkingDirectory = fromDirectory;

            gitProcess.StartInfo = gitInfo;
            gitProcess.Start();
            
            gitProcess.WaitForExit();


            var stderr = gitProcess.StandardError.ReadToEnd();
            var stdout = gitProcess.StandardOutput.ReadToEnd();

            gitProcess.Close();

            return stdout;
        }
    }
}
