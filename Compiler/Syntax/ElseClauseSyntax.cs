namespace Compiler.Syntax;

public sealed class ElseClauseSyntax(SyntaxToken elseKeyword, StatementSyntax elseStatement) : SyntaxNode
{
    public override SyntaxKind Kind => SyntaxKind.ElseClause;
    public SyntaxToken ElseKeyword { get; } = elseKeyword;
    public StatementSyntax ElseStatement { get; } = elseStatement;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ElseKeyword;
        yield return ElseStatement;
    }
}