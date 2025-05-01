using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Sayedha.Analyzers.Shared;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sayedha.Analyzers.Tests2 {
    public class TestHelper {
        public static MetadataReference[] GetAllReferencesNeededForType(Type type) {
            var files = GetAllAssemblyFilesNeededForType(type);

            return files.Select(x => MetadataReference.CreateFromFile(x)).Cast<MetadataReference>().ToArray();
        }

        public static ImmutableArray<string> GetAllAssemblyFilesNeededForType(Type type) {
            return type.Assembly.GetReferencedAssemblies()
                .Select(x => Assembly.Load(x.FullName))
                .Append(type.Assembly)
                .Select(x => x.Location)
                .ToImmutableArray();
        }
        public static async Task<ImmutableArray<Diagnostic>> GetDiagnosticsAsync(string code, DiagnosticAnalyzer[] analyzers, params Type[]typesToAdd) {
            AdhocWorkspace workspace = new AdhocWorkspace();

            var solution = workspace.CurrentSolution;

            var projectId = ProjectId.CreateNewId();

            solution = solution
                .AddProject(
                    projectId,
                    "MyTestProject",
                    "MyTestProject",
                    LanguageNames.CSharp);

            solution = solution
                .AddDocument(DocumentId.CreateNewId(projectId),
                "File.cs",
                code);

            var project = solution.GetProject(projectId);

            foreach(var type in typesToAdd) {
                project = project
                            .AddMetadataReference(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                            .AddMetadataReferences(TestHelper.GetAllReferencesNeededForType(type));
            }

            //project = project.AddMetadataReference(
            //    MetadataReference.CreateFromFile(
            //        typeof(object).Assembly.Location))
            //    .AddMetadataReferences(TestHelper.GetAllReferencesNeededForType(typeof(ImmutableArray)));

            var compilation = await project.GetCompilationAsync();

            var compilationWithAnalyzer = compilation.WithAnalyzers(
                ImmutableArray.Create<DiagnosticAnalyzer>(
                    new ImproperCertificateValidationAnalyzer()));

            var foo = ImmutableArray.Create<DiagnosticAnalyzer>(analyzers);

            var diagnostics = await compilationWithAnalyzer.GetAllDiagnosticsAsync();
            return diagnostics;
        }
        
    }
}
