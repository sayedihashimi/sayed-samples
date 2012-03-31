namespace Examples.Tasks.TaskFactories
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

public class BatchFileTask : Task
{
    public BatchFileTask(string xmlBody)
    {
        this.InitalizeFromXml(xmlBody);
    }

    private string Filepath
    { get; set; }

    public string Message
    { get; set; }

    public int ExitCode
    { get; set; }

    public ITaskItem[] Files
    { get; set; }

    private void InitalizeFromXml(string xmlBody)
    {
        if (!string.IsNullOrWhiteSpace(xmlBody))
        {
            // parse the doc, should look like this <Script Filepath="..."/>
            XDocument doc = XDocument.Parse(xmlBody);
            XNamespace xnamespace = 
                @"http://schemas.microsoft.com/developer/msbuild/2003";
            var node = (from n in doc.Elements(xnamespace + "Script")
                        select n).SingleOrDefault();
            if (node != null)
            {
                this.Filepath = node.Attribute("Filepath").Value;
            }
        }
    }

    public override bool Execute()
    {
        if (!string.IsNullOrWhiteSpace(Filepath))
        {
            // make sure the file exists
            if (!File.Exists(this.Filepath))
            {
                Log.LogError("Batch file not found at [{0}]", this.Filepath);
            }
            else
            {
                Log.LogMessage(
                    MessageImportance.High, 
                    "Executing batch file from [{0}]", 
                    this.Filepath);
                string cmdFilepath = ToolLocationHelper.GetPathToSystemFile("cmd.exe");
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(this.Filepath);
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                int exitCode = process.ExitCode;
                if (exitCode != 0)
                {
                    Log.LogError(
                        "Non-zero exit code [{0}] from batch file [{1}]",
                        exitCode,
                        this.Filepath);
                }
                // you could set this via a parameter
                // process.StartInfo.WorkingDirectory
            }
        }

        return !this.Log.HasLoggedErrors;
    }
}
}
