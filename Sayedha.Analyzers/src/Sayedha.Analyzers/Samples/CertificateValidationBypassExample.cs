using System.Collections.Immutable;
using System.Net.Http;

namespace Sayedha.Analyzers.Samples {
    public class CertificateValidationBypassExample {
        public void MakeRequest() {
            var handler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
            };
            var client = new HttpClient(handler);
            client.GetStringAsync("https://example.com").Wait();

            var array1 = ImmutableArray<int>.Empty.Add(1);
        }
    }
}