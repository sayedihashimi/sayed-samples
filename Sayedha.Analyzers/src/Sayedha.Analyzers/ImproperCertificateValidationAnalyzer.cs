using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq.Expressions;

namespace Sayedha.Analyzers {

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
            if (!System.Diagnostics.Debugger.IsAttached) {
                System.Diagnostics.Debugger.Launch();
            }
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode2, 
                SyntaxKind.SimpleLambdaExpression, 
                SyntaxKind.ParenthesizedLambdaExpression, 
                SyntaxKind.AnonymousMethodExpression);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context) {
            if (context.Node is ParenthesizedLambdaExpressionSyntax lambdaExpression) {
                // Check if the lambda always returns true
                if (lambdaExpression.Body is LiteralExpressionSyntax literalExpression &&
                    literalExpression.IsKind(SyntaxKind.TrueLiteralExpression)) {
                    var parentAssignment = lambdaExpression.Parent as AssignmentExpressionSyntax;
                    if (parentAssignment?.Left is MemberAccessExpressionSyntax memberAccess &&
                        memberAccess.Name.Identifier.Text == "ServerCertificateCustomValidationCallback" &&
                        memberAccess.Expression is IdentifierNameSyntax identifierName &&
                        identifierName.Identifier.Text == "HttpClientHandler") {
                        var diagnostic = Diagnostic.Create(Rule, lambdaExpression.GetLocation());
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        private static void AnalyzeNode2(SyntaxNodeAnalysisContext context) {
            var node = (ParenthesizedLambdaExpressionSyntax)context.Node;

            if(!(node.Body is LiteralExpressionSyntax literalExpression &&
                literalExpression.IsKind(SyntaxKind.TrueLiteralExpression))) {
                return;
            }

            if (!(node.Parent is AssignmentExpressionSyntax parentAssignment)) {
                return;
            }

            if(!(parentAssignment?.Left is IdentifierNameSyntax idName)) {
                return;
            }

            context.ReportDiagnostic(
                Diagnostic.Create(Rule,
                node.Parent.GetLocation()));


        }
    }

}