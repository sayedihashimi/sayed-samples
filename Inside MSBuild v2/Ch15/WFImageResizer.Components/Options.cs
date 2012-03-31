using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ImageResizer.Components
{
    public class Options
    {
        private List<FileInfo> files;

        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public bool AutoRotate { get; set; }
        public bool Parallelize { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public List<FileInfo> Files
        {
            get
            {
                if (files == null || files.Count == 0)
                {
                    files = new List<FileInfo>();
                    string[] imagepaths = Directory.GetFiles(SourceDirectory, "*.jpg");

                    foreach (string path in imagepaths)
                    {
                        FileInfo fileInfo = new FileInfo(path);
                        files.Add(fileInfo);
                    }
                }

                return files;
            }
        }
    }
}
