using Microsoft.CodeAnalysis.Diagnostics;
using Sayedha.Analyzers.Shared;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sayedha.Analyzers.Tests {
    public class ImproperCertificateValidationAnalyzerTests {
        [Fact]
        public async Task TestLiteralTrueAssignment() {
            var code = @"
using System.Net.Http;
public static class Program {
    public static void Main() {
        var handler = new HttpClientHandler {
            ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
        };
    }
}";

            DiagnosticAnalyzer[] analyzers = new[]{ new ImproperCertificateValidationAnalyzer() };
            var diagnostics = await TestHelper.GetDiagnosticsAsync(code, analyzers, 
                typeof(object), typeof(ImproperCertificateValidationAnalyzer),typeof(HttpClientHandler));

            Assert.Single(diagnostics);
            Assert.Equal("ICV001", diagnostics[0].Id);
        }

        // I can't get this test to recognize the Func<> class for some reason.
        // Error: error CS0246: The type or namespace name 'Func<,,,,>' could not be found (are you missing a using directive or an assembly reference?)
        [Fact]
        public async Task TestLiteralTrueAssignmentFromMethod() {
            var code = @"
using System;
using System.Net.Http;
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
        client.GetStringAsync(""https://example.com"").Wait();
    }
    private static Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> GetServerCallback() {
        return (_, __, ___, ____) => true;
    }
}";

            DiagnosticAnalyzer[] analyzers = new[] { new ImproperCertificateValidationAnalyzer() };
            var diagnostics = await TestHelper.GetDiagnosticsAsync(code, analyzers, 
                typeof(object),
                typeof(ImproperCertificateValidationAnalyzer), 
                typeof(HttpClientHandler), 
                typeof(HttpRequestMessage),
                typeof(X509Certificate2),
                typeof(X509Chain),
                typeof(SslPolicyErrors),
                typeof(Uri),
                typeof(System.Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>));

            Assert.Equal(2, diagnostics.Length);
            foreach(var diag in diagnostics) {
                Assert.Equal("ICV001", diag.Id);
            }

            
        }
    }
}