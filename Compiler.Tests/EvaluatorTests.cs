using Compiler.Diagnostics;
using Compiler.Syntax;

namespace Compiler.Tests;

public class EvaluatorTests
{
    [Theory]
    [InlineData("1", 1.0d)]
    [InlineData("+1", 1.0d)]
    [InlineData("-1", -1.0d)]
    [InlineData("1 + 2", 3.0d)]
    [InlineData("1 - 2", -1.0d)]
    [InlineData("1 * 2", 2.0d)]
    [InlineData("6 / 3", 2.0d)]
    [InlineData("2 ** 3", 8.0d)]
    [InlineData("(1 + 2) * 3", 9.0d)]
    [InlineData("1 == 1", true)]
    [InlineData("1 == 2", false)]
    [InlineData("1 != 1", false)]
    [InlineData("1 != 2", true)]
    [InlineData("1 < 2", true)]
    [InlineData("1 > 2", false)]
    [InlineData("1 <= 1", true)]
    [InlineData("1 >= 1", true)]
    public void Evaluator_Evaluates_Expression(string text, object expectedValue)
    {
        // Arrange
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);
        var boundExpression = binder.BindExpression(syntaxTree.Root);
        var evaluator = new Evaluator(boundExpression, diagnostics, variables);

        // Act
        var result = evaluator.Evaluate();

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void Evaluator_Evaluates_Assignment()
    {
        // Arrange
        var text = "a = 10";
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);
        var boundExpression = binder.BindExpression(syntaxTree.Root);
        var evaluator = new Evaluator(boundExpression, diagnostics, variables);

        // Act
        var result = evaluator.Evaluate();

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        Assert.Equal(10.0d, result);
        Assert.Equal(10.0d, variables["a"]);
    }

    [Fact]
    public void Evaluator_Evaluates_Variable()
    {
        // Arrange
        var text = "a = 10";
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);
        var boundExpression = binder.BindExpression(syntaxTree.Root);
        var evaluator = new Evaluator(boundExpression, diagnostics, variables);
        evaluator.Evaluate(); // Assign the variable

        var text2 = "a";
        var syntaxTree2 = SyntaxTree.Parse(text2);
        var boundExpression2 = binder.BindExpression(syntaxTree2.Root);
        var evaluator2 = new Evaluator(boundExpression2, diagnostics, variables);

        // Act
        var result = evaluator2.Evaluate();

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        Assert.Equal(10.0d, result);
    }
}