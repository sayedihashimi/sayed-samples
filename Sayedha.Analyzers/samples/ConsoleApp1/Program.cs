using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class Program {
    public static void Main(string[] args) {
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
    private static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool> GetServerCallback() {
        return (_, __, ___, ____) => true;
    }
}
