using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sayedha.Analyzers.Analyzers {
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CreationAnalyzer : DiagnosticAnalyzer {
        private static DiagnosticDescriptor DiagnosticDescriptor =
            new DiagnosticDescriptor(
                "BadWayOfCreatingImmutableArray",
                "Bad way of creating immutable array",
                "Bad way of creating immutable array",
                "Immutable arrays",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor>
            SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptor);

        //ImmutableArray<int>.Empty.Add(1);

        public override void Initialize(AnalysisContext context) {
            context.RegisterSyntaxNodeAction(
                Analyze,
                SyntaxKind.InvocationExpression);

        }

        private void Analyze(SyntaxNodeAnalysisContext context) {
            var node = (InvocationExpressionSyntax)context.Node;

            if (node.ArgumentList.Arguments.Count != 1)
                return;

            if (!(node.Expression is MemberAccessExpressionSyntax addAccess))
                return;

            if (addAccess.Name.Identifier.Text != "Add")
                return;

            if (!(addAccess.Expression is MemberAccessExpressionSyntax emptyAccess))
                return;

            if (emptyAccess.Name.Identifier.Text != "Empty")
                return;

            if (!(emptyAccess.Expression is GenericNameSyntax immutableArray))
                return;

            if (immutableArray.TypeArgumentList.Arguments.Count != 1)
                return;

            if (immutableArray.Identifier.Text != "ImmutableArray")
                return;

            context.ReportDiagnostic(
                Diagnostic.Create(
                    DiagnosticDescriptor,
                    node.GetLocation()));
        }
    }
}