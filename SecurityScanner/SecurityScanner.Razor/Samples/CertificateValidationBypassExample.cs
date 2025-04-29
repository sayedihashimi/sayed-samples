using System.Net.Http;

namespace SecurityScanner.Razor.Samples;
public class CertificateValidationBypassExample
{
    public void MakeRequest()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
        };
        var client = new HttpClient(handler);
        client.GetStringAsync("https://example.com").Wait();
    }
}
