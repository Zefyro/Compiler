namespace Lexer;

public sealed class VariableExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.VariableExpression;
    public SyntaxToken VariableToken { get; }
    public VariableExpressionSyntax(SyntaxToken variableToken)
    {
        VariableToken = variableToken;
    }
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return VariableToken;
    }
}
