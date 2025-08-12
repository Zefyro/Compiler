using System.Collections.Immutable;

namespace Compiler.Syntax;

public sealed class BlockStatementSyntax(SyntaxToken openBraceToken, ImmutableArray<StatementSyntax> statements, SyntaxToken closeBraceToken) : StatementSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BlockStatement;
    public SyntaxToken OpenBraceToken { get; } = openBraceToken;
    public ImmutableArray<StatementSyntax> Statements { get; } = statements;
    public SyntaxToken CloseBraceToken { get; } = closeBraceToken;

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBraceToken;
        foreach (var statement in Statements)
            yield return statement;
        yield return CloseBraceToken;
    }
}