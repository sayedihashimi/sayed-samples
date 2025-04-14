using System.Net.Http;

namespace SecurityScanner.Razor;
public class CertificateValidationBypassExample
{
    // SECURITY ISSUE: Improper certificate validation
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
