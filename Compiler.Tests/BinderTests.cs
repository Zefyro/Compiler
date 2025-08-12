using Compiler.Binding;
using Compiler.Diagnostics;
using Compiler.Syntax;

namespace Compiler.Tests;

public class BinderTests
{
    [Fact]
    public void Binder_Binds_LiteralExpression()
    {
        // Arrange
        var text = "123";
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);

        // Act
        var boundExpression = binder.BindExpression(((ExpressionStatementSyntax)syntaxTree.Root).Expression);

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        var literal = Assert.IsType<BoundLiteralExpression>(boundExpression);
        Assert.Equal(123.0d, literal.Value);
        Assert.Equal(typeof(double), literal.Type);
    }

    [Fact]
    public void Binder_Binds_UnaryExpression()
    {
        // Arrange
        var text = "-10";
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);

        // Act
        var boundExpression = binder.BindExpression(((ExpressionStatementSyntax)syntaxTree.Root).Expression);

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        var unary = Assert.IsType<BoundUnaryExpression>(boundExpression);
        Assert.Equal(BoundKind.UnaryMinusExpression, unary.OperatorKind);
        Assert.Equal(typeof(double), unary.Type);

        var literal = Assert.IsType<BoundLiteralExpression>(unary.Operand);
        Assert.Equal(10.0d, literal.Value);
    }

    [Fact]
    public void Binder_Binds_BinaryExpression()
    {
        // Arrange
        var text = "1 + 2";
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);

        // Act
        var boundExpression = binder.BindExpression(((ExpressionStatementSyntax)syntaxTree.Root).Expression);

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        var binary = Assert.IsType<BoundBinaryExpression>(boundExpression);
        Assert.Equal(BoundKind.BinaryAdditionExpression, binary.OperatorKind);
        Assert.Equal(typeof(double), binary.Type);

        var left = Assert.IsType<BoundLiteralExpression>(binary.Left);
        Assert.Equal(1.0d, left.Value);

        var right = Assert.IsType<BoundLiteralExpression>(binary.Right);
        Assert.Equal(2.0d, right.Value);
    }

    [Fact]
    public void Binder_Binds_AssignmentExpression()
    {
        // Arrange
        var text = "a = 10";
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);

        // Act
        var boundExpression = binder.BindExpression(((ExpressionStatementSyntax)syntaxTree.Root).Expression);

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        var assignment = Assert.IsType<BoundAssignmentExpression>(boundExpression);
        Assert.Equal("a", assignment.VariableName);
        Assert.Equal(typeof(double), assignment.Type);

        var assignedExpression = Assert.IsType<BoundLiteralExpression>(assignment.Expression);
        Assert.Equal(10.0d, assignedExpression.Value);
    }

    [Fact]
    public void Binder_Binds_BooleanComparison()
    {
        // Arrange
        var text = "1 == 2";
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);

        // Act
        var boundExpression = binder.BindExpression(((ExpressionStatementSyntax)syntaxTree.Root).Expression);

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        var binary = Assert.IsType<BoundBinaryExpression>(boundExpression);
        Assert.Equal(BoundKind.BinaryEqualsExpression, binary.OperatorKind);
        Assert.Equal(typeof(bool), binary.Type);
    }

    [Fact]
    public void Binder_Binds_BooleanNotEquals()
    {
        // Arrange
        var text = "1 != 2";
        var syntaxTree = SyntaxTree.Parse(text);
        var diagnostics = new DiagnosticBag();
        var variables = new Dictionary<string, object>();
        var binder = new Binder(diagnostics, variables);

        // Act
        var boundExpression = binder.BindExpression(((ExpressionStatementSyntax)syntaxTree.Root).Expression);

        // Assert
        Assert.Empty(diagnostics.ToImmutableArray());
        var binary = Assert.IsType<BoundBinaryExpression>(boundExpression);
        Assert.Equal(BoundKind.BinaryNotEqualsExpression, binary.OperatorKind);
        Assert.Equal(typeof(bool), binary.Type);
    }
}