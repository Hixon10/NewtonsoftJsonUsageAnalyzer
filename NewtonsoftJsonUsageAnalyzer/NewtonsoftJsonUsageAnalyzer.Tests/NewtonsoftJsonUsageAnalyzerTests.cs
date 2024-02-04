using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
        NewtonsoftJsonUsageAnalyzer.NewtonsoftJsonUsageAnalyzer>;

namespace NewtonsoftJsonUsageAnalyzer.Tests;

public class NewtonsoftJsonUsageAnalyzerTests
{
    [Fact]
    public async Task TestWithNewtonsoftUsing()
    {
        const string text = @"
using Newtonsoft.Json;

namespace HelloWorld;
public class MyCompanyClass
{
}
";
        await ExecuteTest(text, "Newtonsoft.Json");
    }
    
    [Fact]
    public async Task TestWithNewtonsoftLowercaseUsing()
    {
        const string text = @"
using newtonsoft.Json;

namespace HelloWorld;
public class MyCompanyClass
{
}
";
        await ExecuteTest(text, "newtonsoft.Json");
    }
    
    [Fact]
    public async Task TestWithNewtonsoftUppercaseUsing()
    {
        const string text = @"
using NEWtonsoft.Json;

namespace HelloWorld;
public class MyCompanyClass
{
}
";
        await ExecuteTest(text, "NEWtonsoft.Json");
    }
    
    [Fact]
    public async Task TestWithNewtonsoftLinqUsing()
    {
        const string text = @"
using Newtonsoft.Json.Linq;

namespace HelloWorld;
public class MyCompanyClass
{
}
";
        await ExecuteTest(text, "Newtonsoft.Json.Linq");
    }

    [Fact]
    public async Task TestWithoutNewtonsoftUsing()
    {
        const string text = """
                            namespace HelloWorld;
                            public class MyCompanyClass
                            {
                            }
                            """;

        var test = new CSharpAnalyzerTest<NewtonsoftJsonUsageAnalyzer, XUnitVerifier>
        {
            TestCode = text,
            CompilerDiagnostics = CompilerDiagnostics.None
        };

        await test.RunAsync(CancellationToken.None);
    }
    
    private static async Task ExecuteTest(string textCode, string packageName)
    {
        var expected1 = Verifier.Diagnostic()
            .WithLocation(2, 1)
            .WithMessage($"Detected Newtonsoft usage: '{packageName}'")
            .WithArguments(packageName);
        
        var test = new CSharpAnalyzerTest<NewtonsoftJsonUsageAnalyzer, XUnitVerifier>
        {
            TestCode = textCode,
            CompilerDiagnostics = CompilerDiagnostics.None
        };
        test.ExpectedDiagnostics.Add(expected1);

        await test.RunAsync(CancellationToken.None);
    }
}