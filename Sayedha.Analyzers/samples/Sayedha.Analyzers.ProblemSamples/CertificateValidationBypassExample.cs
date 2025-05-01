using System.Collections.Immutable;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Sayedha.Analyzers.Samples {
    public class CertificateValidationBypassExample {
        public void MakeRequest() {
            // this will be detected
            var handler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
            };

            // this will NOT be detected
            var h2 = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = GetServerCallback()
            };

            var client = new HttpClient(handler);
            client.GetStringAsync("https://example.com").Wait();
        }

        private Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool> GetServerCallback() {
            return (_, __, ___, ____) => true;
        }
    }
}