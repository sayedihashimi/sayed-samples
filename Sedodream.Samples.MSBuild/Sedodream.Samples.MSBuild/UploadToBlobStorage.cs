namespace Sedodream.Samples.MSBuild {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public class UploadToBlobStorage : Task {
        public UploadToBlobStorage() {
            this.CreateContainerIfNotExists = true;
        }

        [Required]
        public string StorageConnectionString { get; set; }

        [Required]
        public string ContainerName { get; set; }

        public string BlobName { get; set; }

        [Required]
        public string FileToUpload { get; set; }

        [Output]
        public string BlobUri { get; set; }

        /// <summary>
        /// Defaults to true
        /// </summary>
        public bool CreateContainerIfNotExists { get; set; }

        public override bool Execute() {
            if (!File.Exists(this.FileToUpload)) {
                string message = string.Format("File to upload [{0}] not found",this.FileToUpload);
                throw new FileNotFoundException(message);
            }

            try {
                CloudStorageAccount storageAcct = CloudStorageAccount.Parse(this.StorageConnectionString);
                CloudBlobClient client = storageAcct.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(this.ContainerName);

                if (this.CreateContainerIfNotExists) {
                    container.CreateIfNotExist();
                }

                if (string.IsNullOrEmpty(this.BlobName)) {
                    FileInfo fileInfo = new FileInfo(this.FileToUpload);
                    this.BlobName = fileInfo.Name;
                }

                CloudBlob blob = container.GetBlobReference(this.BlobName);
                BlobRequestOptions options = new BlobRequestOptions();
                options.Timeout = new TimeSpan(0, 0, 15);
                blob.UploadFile(this.FileToUpload);

                this.BlobUri = blob.Uri.AbsoluteUri;

            }
            catch (Exception ex) {
                string message = "An error occurred trying to upload the file to blob storage.";
                throw new LoggerException(message, ex);
            }
            return true;
        }
    }
}
