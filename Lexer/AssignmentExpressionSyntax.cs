namespace Lexer;

public sealed class AssignmentExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;

    public SyntaxToken VariableToken { get; }
    public SyntaxToken EqualsToken { get; }
    public ExpressionSyntax Expression { get; }

    public AssignmentExpressionSyntax(SyntaxToken variableToken, SyntaxToken equalsToken, ExpressionSyntax expression)
    {
        VariableToken = variableToken;
        EqualsToken = equalsToken;
        Expression = expression;
    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return VariableToken;
        yield return EqualsToken;
        yield return Expression;
    }
}
