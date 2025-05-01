using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
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
            context.RegisterSyntaxNodeAction(AnalyzeLambdaNode, 
                SyntaxKind.SimpleLambdaExpression, 
                SyntaxKind.ParenthesizedLambdaExpression, 
                SyntaxKind.AnonymousMethodExpression);
            
            // Register for method invocation expressions to detect cases where a method is called
            // that returns a callback which always returns true
            context.RegisterSyntaxNodeAction(AnalyzeInvocationNode, SyntaxKind.InvocationExpression);
        }

        /// <summary>
        /// This will check for: 
        /// 
        /// var handler = new HttpClientHandler {
        ///     ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
        /// }
        /// </summary>
        /// <param name="context"></param>
        private static void AnalyzeLambdaNode(SyntaxNodeAnalysisContext context) {
            // Handle different lambda expression types
            LambdaExpressionSyntax node;
            if (context.Node is ParenthesizedLambdaExpressionSyntax parenthesizedLambda) {
                node = parenthesizedLambda;
            } else if (context.Node is SimpleLambdaExpressionSyntax simpleLambda) {
                node = simpleLambda;
            } else if (context.Node is AnonymousMethodExpressionSyntax anonymousMethod) {
                // For anonymous methods, check the body
                if (anonymousMethod.Block != null) {
                    // If there's a single return statement with "true", flag it
                    var returnStatements = anonymousMethod.Block.Statements
                        .OfType<ReturnStatementSyntax>()
                        .ToList();
                    
                    if (returnStatements.Count == 1 && 
                        returnStatements[0].Expression is LiteralExpressionSyntax literalExpr &&
                        literalExpr.IsKind(SyntaxKind.TrueLiteralExpression)) {
                        
                        CheckIfAssignedToServerCertificateCallback(context, anonymousMethod);
                    }
                }
                return;
            } else {
                return;
            }

            // Check if the lambda returns true
            if (node.Body is LiteralExpressionSyntax literalExpression && 
                literalExpression.IsKind(SyntaxKind.TrueLiteralExpression)) {
                // Check if this lambda is assigned to ServerCertificateCustomValidationCallback
                CheckIfAssignedToServerCertificateCallback(context, node);
            } else if (node.Body is BlockSyntax block) {
                // For lambdas with a block body, check if it has a single return true statement
                var returnStatements = block.Statements
                    .OfType<ReturnStatementSyntax>()
                    .ToList();
                
                if (returnStatements.Count == 1 && 
                    returnStatements[0].Expression is LiteralExpressionSyntax literal &&
                    literal.IsKind(SyntaxKind.TrueLiteralExpression)) {
                    
                    CheckIfAssignedToServerCertificateCallback(context, node);
                }
            }
        }

        /// <summary>
        /// Analyzes invocation expressions that might be assigned to ServerCertificateCustomValidationCallback
        /// </summary>
        /// <param name="context"></param>
        private static void AnalyzeInvocationNode(SyntaxNodeAnalysisContext context) {
            var invocation = (InvocationExpressionSyntax)context.Node;

            // Check if this invocation is part of an assignment
            if (!(invocation.Parent is AssignmentExpressionSyntax parentAssignment)) {
                return;
            }

            // Check if left side is ServerCertificateCustomValidationCallback
            if (!IsServerCertificateCallbackProperty(context, parentAssignment.Left)) {
                return;
            }

            // Get the symbol for the method being called
            var methodSymbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
            if (methodSymbol == null) {
                return;
            }

            // Find the method declaration
            var methodDeclaration = FindMethodDeclaration(context, methodSymbol);
            if (methodDeclaration == null) {
                return;
            }

            // Analyze the method to see if it returns a lambda that always returns true
            if (ReturnsAlwaysTruePredicate(context, methodDeclaration)) {
                context.ReportDiagnostic(
                    Diagnostic.Create(Rule, parentAssignment.GetLocation()));
            }
        }

        /// <summary>
        /// Finds the method declaration for a given method symbol
        /// </summary>
        private static MethodDeclarationSyntax FindMethodDeclaration(SyntaxNodeAnalysisContext context, IMethodSymbol methodSymbol) {
            // Try to get the declaration syntax reference
            var syntaxRef = methodSymbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (syntaxRef == null) {
                return null;
            }

            // Get the syntax node from the reference
            var node = syntaxRef.GetSyntax();
            if (node is MethodDeclarationSyntax methodDeclaration) {
                return methodDeclaration;
            }

            return null;
        }

        /// <summary>
        /// Analyzes a method to determine if it returns a lambda that always returns true
        /// </summary>
        private static bool ReturnsAlwaysTruePredicate(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration) {
            // Check if the method has a single return statement
            if (methodDeclaration.Body == null) {
                return false;
            }

            // Look for return statements
            var returnStatements = methodDeclaration.Body.Statements
                .OfType<ReturnStatementSyntax>()
                .ToList();

            // For simplicity, we'll focus on methods with a single return statement
            if (returnStatements.Count != 1 || returnStatements[0].Expression == null) {
                return false;
            }

            var returnExpr = returnStatements[0].Expression;

            // Case 1: Directly returning a lambda that returns true
            if (returnExpr is ParenthesizedLambdaExpressionSyntax lambdaExpr && 
                lambdaExpr.Body is LiteralExpressionSyntax literalExpr &&
                literalExpr.IsKind(SyntaxKind.TrueLiteralExpression)) {
                return true;
            }

            // Case 2: Returning a lambda with a block that returns true
            if (returnExpr is ParenthesizedLambdaExpressionSyntax blockLambda && 
                blockLambda.Body is BlockSyntax lambdaBlock) {
                
                var lambdaReturns = lambdaBlock.Statements
                    .OfType<ReturnStatementSyntax>()
                    .ToList();
                
                if (lambdaReturns.Count == 1 && 
                    lambdaReturns[0].Expression is LiteralExpressionSyntax lambdaReturnLiteral &&
                    lambdaReturnLiteral.IsKind(SyntaxKind.TrueLiteralExpression)) {
                    return true;
                }
            }

            // Case 3: Simplified lambda that returns true
            if (returnExpr is SimpleLambdaExpressionSyntax simpleLambda && 
                simpleLambda.Body is LiteralExpressionSyntax simpleLiteralExpr &&
                simpleLiteralExpr.IsKind(SyntaxKind.TrueLiteralExpression)) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a node is assigned to ServerCertificateCustomValidationCallback and reports diagnostics if needed
        /// </summary>
        private static void CheckIfAssignedToServerCertificateCallback(SyntaxNodeAnalysisContext context, SyntaxNode node) {
            // parent node must be an assignment
            if (!(node.Parent is AssignmentExpressionSyntax parentAssignment)) {
                return;
            }

            // Check if the left side is the ServerCertificateCustomValidationCallback property
            if (!IsServerCertificateCallbackProperty(context, parentAssignment.Left)) {
                return;
            }

            context.ReportDiagnostic(
                Diagnostic.Create(Rule, parentAssignment.GetLocation()));
        }

        /// <summary>
        /// Determines if the node represents the ServerCertificateCustomValidationCallback property of HttpClientHandler
        /// </summary>
        private static bool IsServerCertificateCallbackProperty(SyntaxNodeAnalysisContext context, SyntaxNode node) {
            // Left side can be either a simple identifier or a member access expression
            IdentifierNameSyntax idName = null;
            
            if (node is IdentifierNameSyntax identifierName) {
                idName = identifierName;
            } else if (node is MemberAccessExpressionSyntax memberAccess) {
                idName = memberAccess.Name as IdentifierNameSyntax;
            }

            if (idName == null) {
                return false;
            }

            // Get symbol for the identifier
            if (!(context.SemanticModel.GetSymbolInfo(idName).Symbol is IPropertySymbol sym)) {
                return false;
            }

            // Check that the type is System.Net.Http.HttpClientHandler and that the
            // property being assigned is ServerCertificateCustomValidationCallback
            return GetFullnameNamespace(sym.ContainingNamespace).Equals("System.Net.Http") &&
                   sym.ContainingType.Name.Equals("HttpClientHandler") &&
                   sym.Name.Equals("ServerCertificateCustomValidationCallback");
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