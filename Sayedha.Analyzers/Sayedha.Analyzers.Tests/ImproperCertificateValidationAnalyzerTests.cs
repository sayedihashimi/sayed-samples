using Microsoft.CodeAnalysis.Diagnostics;
using Sayedha.Analyzers.Shared;
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
            var diagnostics = await TestHelper.GetDiagnosticsAsync(code, analyzers, typeof(object), typeof(ImproperCertificateValidationAnalyzer),typeof(HttpClientHandler));

            Assert.Single(diagnostics);
        }
    }
}