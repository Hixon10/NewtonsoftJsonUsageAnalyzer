using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NewtonsoftJsonUsageAnalyzer;

/// <summary>
/// An analyzer that reports Newtonsoft library using being used in a class.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NewtonsoftJsonUsageAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "NS0001";

    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.NS0001Title),
        Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.NS0001MessageFormat), Resources.ResourceManager,
            typeof(Resources));

    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.NS0001Description), Resources.ResourceManager,
            typeof(Resources));

    // The category of the diagnostic (Design, Naming etc.).
    private const string Category = "Design";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        // Disable analyzing generated code.
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        // Enabling the Concurrent Execution.
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeUsing, SyntaxKind.UsingDirective);
    }
    
    private static void AnalyzeUsing(SyntaxNodeAnalysisContext context)
    {
        var usingDirective = (UsingDirectiveSyntax)context.Node;
        string directive = usingDirective.Name.ToString();
        if (directive.IndexOf("newtonsoft", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            var diagnostic = Diagnostic.Create(Rule, usingDirective.GetLocation(), directive);
            context.ReportDiagnostic(diagnostic);
        }
    }
}