; Shipped analyzer releases
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 0.1.0.2

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
AB0001 | Design | Error | C# events can cause memory leaks in an NSObject subclass. Remove the event or add the [UnconditionalSuppressMessage("Memory", "MA0001")] attribute with a justification as to why the event will not leak.
