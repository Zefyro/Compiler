namespace Compiler.Syntax;

public sealed class IfStatementSyntax(SyntaxToken ifKeyword, ExpressionSyntax condition, StatementSyntax thenStatement, ElseClauseSyntax? elseClause) : StatementSyntax
{
    public override SyntaxKind Kind => SyntaxKind.IfStatement;
    public SyntaxToken IfKeyword { get; } = ifKeyword;
    public ExpressionSyntax Condition { get; } = condition;
    public StatementSyntax ThenStatement { get; } = thenStatement;
    public ElseClauseSyntax? ElseClause { get; } = elseClause;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IfKeyword;
        yield return Condition;
        yield return ThenStatement;
        if (ElseClause is not null)
            yield return ElseClause;
    }
}