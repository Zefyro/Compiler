namespace Compiler.Syntax;
public sealed class AssignmentExpressionSyntax(SyntaxToken variableToken, SyntaxToken equalsToken, ExpressionSyntax expression) : ExpressionSyntax {
    public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;
    public SyntaxToken VariableToken { get; } = variableToken;
    public SyntaxToken EqualsToken { get; } = equalsToken;
    public ExpressionSyntax Expression { get; } = expression;
    public override IEnumerable<SyntaxNode> GetChildren() {
        yield return VariableToken;
        yield return EqualsToken;
        yield return Expression;
    }
}
