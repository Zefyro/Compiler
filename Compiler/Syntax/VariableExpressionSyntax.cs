namespace Compiler.Syntax;
public sealed class VariableExpressionSyntax(SyntaxToken variableToken) : ExpressionSyntax {
    public override SyntaxKind Kind => SyntaxKind.VariableExpression;
    public SyntaxToken VariableToken { get; } = variableToken;
    public override IEnumerable<SyntaxNode> GetChildren() {
        yield return VariableToken;
    }
}
