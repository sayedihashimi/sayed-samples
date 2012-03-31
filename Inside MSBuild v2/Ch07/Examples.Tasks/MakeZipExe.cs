namespace Examples.Tasks
{
    using System.IO;
    using System.Text;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.Win32;

    public class MakeZipExe : ToolTask
    {
        private const string ExeName = "makezipexe.exe";

        public MakeZipExe()
        {
            Overwrite = false;
        }

        [Required]
        public ITaskItem Zipfile { get; set; }
        public ITaskItem OutputFile { get; set; }
        public bool Overwrite { get; set; }

        protected override bool ValidateParameters()
        {
            base.Log.LogMessageFromText("Validating arguments", MessageImportance.Low);

            if (!File.Exists(Zipfile.GetMetadata("FullPath")))
            {
                string message = string.Format("Missing ZipFile: {0}", Zipfile);
                base.Log.LogError(message, null);
                return false;
            }
            if (File.Exists(OutputFile.GetMetadata("FullPath")) && !Overwrite)
            {
                string message = string.Format("Output file {0}, Overwrite false.",
                    OutputFile);
                base.Log.LogError(message, null);
                return false;
            }

            return base.ValidateParameters();
        }

        protected override string GenerateFullPathToTool()
        {
            string path = ToolPath;
            // If ToolPath was not provided by the MSBuild script try to find it.
            if (string.IsNullOrEmpty(path))
            {
                string regKey = @"SOFTWARE\Microsoft\VisualStudio\9.0\Setup\VS";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regKey))
                {
                    if (key != null)
                    {
                        string keyValue =
                            key.GetValue("EnvironmentDirectory", null).ToString();
                        path = keyValue;
                    }
                }
            }
            if (string.IsNullOrEmpty(path))
            {

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey
                    (@"SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VS"))
                {
                    if (key != null)
                    {
                        string keyValue =
                            key.GetValue("EnvironmentDirectory", null).ToString();
                        path = keyValue;
                    }
                }

            }
            if (string.IsNullOrEmpty(path))
            {
                Log.LogError("VisualStudio install directory not found",
                    null);

                return string.Empty;
            }
            string fullpath = Path.Combine(path, ToolName);
            return fullpath;
        }

        protected override string GenerateCommandLineCommands()
        {
            StringBuilder sb = new StringBuilder();
            if (Zipfile != null)
            {
                sb.Append(
                    string.Format("-zipfile:{0} ",
                    Zipfile.GetMetadata("FullPath")));
            }
            if (OutputFile != null)
            {
                sb.Append(
                    string.Format("-output:{0} ",
                    OutputFile.GetMetadata("FullPath")));
            }
            if (Overwrite)
                sb.Append("-overwrite:true ");

            return sb.ToString();
        }
        protected override string ToolName
        {
            get { return ExeName; }
        }
    }
}
