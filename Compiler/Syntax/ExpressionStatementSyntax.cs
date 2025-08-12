namespace Compiler.Syntax;

public sealed class ExpressionStatementSyntax(ExpressionSyntax expression) : StatementSyntax
{
    public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;
    public ExpressionSyntax Expression { get; } = expression;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Expression;
    }
}