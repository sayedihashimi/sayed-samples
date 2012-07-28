namespace ConsoleApplication1 {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using MvcApplication1.Models;


    public class Program {
        public static void Main(string[] args) {

            string url = @"http://localhost:10512/api/values";
            

            Person personToPost = new Person {
                FirstName = "first name here",
                LastName = "last name here",
                Id = new Random().Next()
            };


            var jsonFormatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            var mediaType = new MediaTypeHeaderValue("application/json");
            var requestMessage = new HttpRequestMessage<Person>(
                personToPost,
                mediaType,
                new MediaTypeFormatter[] { jsonFormatter });

            HttpClient client = null;
            HttpResponseMessage response = null;
            try {
                client = new HttpClient();
                Task postTask = client.PostAsync(url, requestMessage.Content).ContinueWith(respMsg => {
                    response = respMsg.Result;
                });

                postTask.Wait();

                Console.WriteLine(string.Format("Response status code: {0}", response.StatusCode));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex) {
                string message = string.Format("An error occurred: {0}{1}{0}]", Environment.NewLine, ex.Message);
            }
            finally {
                if (client != null) {
                    client.Dispose();
                    client = null;
                }
            }

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
