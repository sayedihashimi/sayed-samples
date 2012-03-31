namespace Examples.Tasks
{
    using System;
    using System.IO;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    /// <summary>
    /// This is an MSBuild move task, similar to the Copy task, but the source file
    /// will be deleted upon successful move.
    /// 
    /// Sayed Ibrahim Hashimi (sayed.hashimi@gmail.com)
    /// www.sedodream.com
    /// </summary>
    public class Move : Task
    {
        #region MSBuild properties
        /// <summary>
        /// Contains the list of files to move
        /// </summary>
        [Required]
        public ITaskItem[] SourceFiles
        { get; set; }
        /// <summary>
        /// Item that contains where to move the files to.
        /// Either this or the <code>DestinationFolder</code> parameter should be used.
        /// If both are provided then an error will be rasied.
        /// </summary>
        public ITaskItem[] DestinationFiles
        { get; set; }
        /// <summary>
        /// This is the folder where the files should be moved to.
        /// This can be used instead of the <code>DestinationFiles</code> parameter.
        /// Only one of these parameters should be defined, if both are present then
        /// and error will be raised.
        /// </summary>
        public ITaskItem DestinationFolder
        { get; set; }
        /// <summary>
        /// This contains the list of files that were actually moved.
        /// </summary>
        [Output]
        public ITaskItem[] MovedFiles
        { get; private set; }
        /// <summary>
        /// This contains the length of the files moved.
        /// This really should be placed as Metadata on the <c>MovedFiles</c>
        /// but this is placed here to demonstrate that types other than
        /// <c>ITaskItem</c> can be passed in/out of tasks.
        /// </summary>
        [Output]
        public long[] FileLengths
        { get; private set; }
        #endregion

        public override bool Execute()
        {
            
            Log.LogMessageFromText("Starting move", MessageImportance.Normal);
            bool allSucceeded = true;

            if (this.SourceFiles == null || this.SourceFiles.Length <= 0)
            {
                //if nothing to move just leave quietly
                this.DestinationFiles = new ITaskItem[0];
                Log.LogMessageFromText("Nothing to move", MessageImportance.Normal);
                return true;
            }

            if (this.DestinationFiles == null && this.DestinationFolder == null)
            {
                Log.LogError("Unable to determine destination for files");
                return false;
            }
            if (this.DestinationFiles != null && this.DestinationFolder != null)
            {
                Log.LogError("Both DestinationFiles & DestinationFolder were specified, only one can be provided");
                return false;
            }

            if (this.DestinationFiles != null && (this.DestinationFiles.Length != this.SourceFiles.Length))
            {
                //# of items in source & dest don't match up
                Log.LogError("SourceFiles and DestinationFiles list have different sizes");
                return false;
            }

            if (this.DestinationFiles == null)
            {
                //populate from DestinationFolder
                this.DestinationFiles = new ITaskItem[this.SourceFiles.Length];
                this.FileLengths = new long[this.SourceFiles.Length];

                for (int i = 0; i < this.SourceFiles.Length; i++)
                {
                    string destFile;

                    try
                    {
                        destFile = Path.Combine(this.DestinationFolder.ItemSpec,
                            Path.GetFileName(this.SourceFiles[i].ItemSpec));
                    }
                    catch (Exception ex)
                    {
                        Log.LogError("Unable to move files; " + ex.Message, null);
                        this.DestinationFiles = new ITaskItem[0];
                        return false;
                    }
                    this.DestinationFiles[i] = new TaskItem(destFile);
                    this.SourceFiles[i].CopyMetadataTo(this.DestinationFiles[i]);
                    this.FileLengths[i] = new FileInfo(destFile).Length;
                }
            }

            MovedFiles = new ITaskItem[this.SourceFiles.Length];
            //now we can go through and move all the files
            for (int i = 0; i < SourceFiles.Length; i++)
            {
                string sourcePath = this.SourceFiles[i].ItemSpec;
                string destPath = this.DestinationFiles[i].ItemSpec;
                try
                {
                    string message = string.Format("Moving file {0} to {1}",
                        sourcePath, destPath);
                    Log.LogMessageFromText(message, MessageImportance.Normal);
                    FileInfo destFile = new FileInfo(destPath);
                    DirectoryInfo parentDir = destFile.Directory;
                    if (!parentDir.Exists)
                    {
                        parentDir.Create();
                    }
                    File.Move(sourcePath, destPath);
                    MovedFiles[i] = new TaskItem(destPath);
                }
                catch (Exception ex)
                {
                    Log.LogError("Unable to move file: " + sourcePath +
                        " to " + destPath + "\n" + ex.Message);
                    allSucceeded = false;
                }
            }

            return allSucceeded;
        }
    }
}
