using Compiler.Syntax;

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
        var rootStatement = Assert.IsType<ExpressionStatementSyntax>(tree.Root);
        var root = Assert.IsType<BinaryExpressionSyntax>(rootStatement.Expression);

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
        var rootStatement = Assert.IsType<ExpressionStatementSyntax>(tree.Root);
        var root = Assert.IsType<BinaryExpressionSyntax>(rootStatement.Expression);

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
    public void Parser_Parses_AssignmentExpression()
    {
        // Arrange
        var text = "a = 10";
        var tree = SyntaxTree.Parse(text);

        // Assert
        Assert.Empty(tree.Diagnostics);
        var rootStatement = Assert.IsType<ExpressionStatementSyntax>(tree.Root);
        var root = Assert.IsType<AssignmentExpressionSyntax>(rootStatement.Expression);

        var variable = Assert.IsType<SyntaxToken>(root.VariableToken);
        Assert.Equal("a", variable.Text);

        var equals = Assert.IsType<SyntaxToken>(root.EqualsToken);
        Assert.Equal(SyntaxKind.EqualsToken, equals.Kind);

        var expression = Assert.IsType<LiteralExpressionSyntax>(root.Expression);
        Assert.Equal(10.0d, expression.LiteralToken.Value);
    }

    [Fact]
    public void Parser_Parses_UnaryExpression()
    {
        // Arrange
        var text = "-10";
        var tree = SyntaxTree.Parse(text);

        // Assert
        Assert.Empty(tree.Diagnostics);
        var rootStatement = Assert.IsType<ExpressionStatementSyntax>(tree.Root);
        var root = Assert.IsType<UnaryExpressionSyntax>(rootStatement.Expression);

        var op = Assert.IsType<SyntaxToken>(root.OperatorToken);
        Assert.Equal(SyntaxKind.MinusToken, op.Kind);

        var operand = Assert.IsType<LiteralExpressionSyntax>(root.Operand);
        Assert.Equal(10.0d, operand.LiteralToken.Value);
    }

    [Fact]
    public void Parser_Parses_BinaryExpression_Precedence()
    {
        // Arrange
        var text = "1 + 2 * 3";
        var tree = SyntaxTree.Parse(text);

        // Assert
        Assert.Empty(tree.Diagnostics);
        var rootStatement = Assert.IsType<ExpressionStatementSyntax>(tree.Root);
        var root = Assert.IsType<BinaryExpressionSyntax>(rootStatement.Expression);

        var left = Assert.IsType<LiteralExpressionSyntax>(root.Left);
        Assert.Equal(1.0d, left.LiteralToken.Value);

        var op = Assert.IsType<SyntaxToken>(root.OperatorToken);
        Assert.Equal(SyntaxKind.PlusToken, op.Kind);

        var right = Assert.IsType<BinaryExpressionSyntax>(root.Right); // Expect a nested binary expression

        var nestedLeft = Assert.IsType<LiteralExpressionSyntax>(right.Left);
        Assert.Equal(2.0d, nestedLeft.LiteralToken.Value);

        var nestedOp = Assert.IsType<SyntaxToken>(right.OperatorToken);
        Assert.Equal(SyntaxKind.StarToken, nestedOp.Kind);

        var nestedRight = Assert.IsType<LiteralExpressionSyntax>(right.Right);
        Assert.Equal(3.0d, nestedRight.LiteralToken.Value);
    }
}