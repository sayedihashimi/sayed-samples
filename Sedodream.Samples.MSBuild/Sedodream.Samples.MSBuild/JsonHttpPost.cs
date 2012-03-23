namespace Sedodream.Samples.MSBuild {
    using System;
    using System.Json;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using Microsoft.Build.Framework;
    using Newtonsoft.Json;

    public class JsonHttpPost : Microsoft.Build.Utilities.Task {
        private object ResponseLock = new object();
        
        [Required]
        public string Url { get; set; }

        [Required]
        public string PostContent { get; set; }

        public override bool Execute() {
            HttpClient client = null;
            try {
                var jsonFormatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
                var mediaType = new MediaTypeHeaderValue("application/json");
                var jsonSerializerSettings = new JsonSerializerSettings();                
                var requestMessage = new HttpRequestMessage<string>(
                    this.PostContent,
                    mediaType,
                    new MediaTypeFormatter[] { jsonFormatter });

                client = new HttpClient();
                HttpResponseMessage response = null;
                System.Threading.Tasks.Task postTask = client.PostAsync(this.Url, requestMessage.Content).ContinueWith(respMessage => {
                    response = respMessage.Result;
                });

                System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[] { postTask });
                
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex) {
                string message = "Unable to post the message.";
                throw new LoggerException(message,ex);
            }
            finally {
                if (client != null) {
                    client.Dispose();
                    client = null;
                }
            }
        }
    }
}
