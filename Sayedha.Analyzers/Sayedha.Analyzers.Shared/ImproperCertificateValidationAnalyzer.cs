using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq.Expressions;

namespace Sayedha.Analyzers.Shared {

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ImproperCertificateValidationAnalyzer : DiagnosticAnalyzer {
        public const string DiagnosticId = "ICV001";
        private static readonly LocalizableString Title = "Improper Certificate Validation";
        private static readonly LocalizableString MessageFormat = "Improper certificate validation: this handler accepts any SSL/TLS certificate.";
        private static readonly LocalizableString Description = "Avoid setting ServerCertificateCustomValidationCallback to always return true.";
        private const string Category = "Security";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, 
                SyntaxKind.SimpleLambdaExpression, 
                SyntaxKind.ParenthesizedLambdaExpression, 
                SyntaxKind.AnonymousMethodExpression);
        }

        /// <summary>
        /// This will check for: 
        /// 
        /// var handler = new HttpClientHandler {
        ///     ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
        /// }
        /// </summary>
        /// <param name="context"></param>
        private static void AnalyzeNode(SyntaxNodeAnalysisContext context) {
            var node = (ParenthesizedLambdaExpressionSyntax)context.Node;

            // check that we are return True literal to the lamdba expression
            if(!(node.Body is LiteralExpressionSyntax literalExpression &&
                literalExpression.IsKind(SyntaxKind.TrueLiteralExpression))) {
                return;
            }

            // parent node must be an assignment
            if (!(node.Parent is AssignmentExpressionSyntax parentAssignment)) {
                return;
            }

            // left side of the parent node must be a property that is being assigned
            if(!(parentAssignment?.Left is IdentifierNameSyntax idName)) {
                return;
            }

            if (!(context.SemanticModel.GetSymbolInfo(idName).Symbol is IPropertySymbol sym)) {
                return;
            }

            // check that the type is System.Net.Http.HttpClientHandler and that the
            //  property being assigned is ServerCertificateCustomValidationCallback
            if (!(GetFullnameNamespace(sym.ContainingNamespace).Equals("System.Net.Http") &&
                sym.ContainingType.Name.Equals("HttpClientHandler") &&
                sym.Name.Equals("ServerCertificateCustomValidationCallback"))) {
                return;
            }

            context.ReportDiagnostic(
                Diagnostic.Create(Rule,
                node.Parent.GetLocation()));
        }
        private static string GetFullnameNamespace(INamespaceSymbol ns) {
            if (ns.IsGlobalNamespace)
                return "";

            if (ns.ContainingNamespace.IsGlobalNamespace)
                return ns.Name;

            return GetFullnameNamespace(ns.ContainingNamespace) + "." + ns.Name;
        }
    }
}