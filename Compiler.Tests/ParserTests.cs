using Compiler.Syntax;
using Xunit;

namespace Compiler.Tests;

public class ParserTests
{
    [Fact]
    public void Parser_Parses_BinaryExpression()
    {
        // Arrange
        var text = "1 + 2";
        var tree = SyntaxTree.Parse(text);

        // Assert
        Assert.Empty(tree.Diagnostics);
        var root = Assert.IsType<BinaryExpressionSyntax>(tree.Root);

        var left = Assert.IsType<LiteralExpressionSyntax>(root.Left);
        Assert.Equal(1.0d, left.LiteralToken.Value);

        var op = Assert.IsType<SyntaxToken>(root.OperatorToken);
        Assert.Equal(SyntaxKind.PlusToken, op.Kind);

        var right = Assert.IsType<LiteralExpressionSyntax>(root.Right);
        Assert.Equal(2.0d, right.LiteralToken.Value);
    }
    [Fact]
    public void Parser_Parses_ParenthesizedExpression()
    {
        // Arrange
        var text = "3 * (1 / 2)";
        var tree = SyntaxTree.Parse(text);

        // Assert
        Assert.Empty(tree.Diagnostics);
        var root = Assert.IsType<BinaryExpressionSyntax>(tree.Root);

        var left = Assert.IsType<LiteralExpressionSyntax>(root.Left);
        Assert.Equal(3.0d, left.LiteralToken.Value);

        var op = Assert.IsType<SyntaxToken>(root.OperatorToken);
        Assert.Equal(SyntaxKind.StarToken, op.Kind);

        var right = Assert.IsType<ParenthesizedExpressionSyntax>(root.Right);
        var subExpression = Assert.IsType<BinaryExpressionSyntax>(right.Expression);

        var subLeft = Assert.IsType<LiteralExpressionSyntax>(subExpression.Left);
        Assert.Equal(1.0d, subLeft.LiteralToken.Value);

        var subOp = Assert.IsType<SyntaxToken>(subExpression.OperatorToken);
        Assert.Equal(SyntaxKind.SlashToken, subOp.Kind);

        var subRight = Assert.IsType<LiteralExpressionSyntax>(subExpression.Right);
        Assert.Equal(2.0d, subRight.LiteralToken.Value);
    }
    [Fact]
    public void Parser_Parses_BooleanExpression()
    {
        // Arrange
        var text = "1 == 1";
        var tree = SyntaxTree.Parse(text);

        // Assert
        Assert.Empty(tree.Diagnostics);
        var root = Assert.IsType<BooleanExpressionSyntax>(tree.Root);

        var left = Assert.IsType<LiteralExpressionSyntax>(root.Left);
        Assert.Equal(1.0d, left.LiteralToken.Value);

        var op = Assert.IsType<SyntaxToken>(root.OperationToken);
        Assert.Equal(SyntaxKind.EqualsEqualsToken, op.Kind);

        var right = Assert.IsType<LiteralExpressionSyntax>(root.Right);
        Assert.Equal(1.0d, right.LiteralToken.Value);
    }
}
