using System.Collections.Immutable;

namespace Compiler.Binding;

public sealed class BoundBlockStatement(ImmutableArray<BoundStatement> statements) : BoundStatement
{
    public override BoundKind Kind => BoundKind.BlockStatement;
    public ImmutableArray<BoundStatement> Statements { get; } = statements;

    public override IEnumerable<BoundNode> GetChildren()
    {
        foreach (var statement in Statements)
            yield return statement;
    }
}