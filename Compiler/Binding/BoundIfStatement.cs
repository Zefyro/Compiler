namespace Compiler.Binding;

public sealed class BoundIfStatement(BoundExpression condition, BoundStatement thenStatement, BoundStatement? elseStatement) : BoundStatement
{
    public override BoundKind Kind => BoundKind.IfStatement;
    public BoundExpression Condition { get; } = condition;
    public BoundStatement ThenStatement { get; } = thenStatement;
    public BoundStatement? ElseStatement { get; } = elseStatement;

    public override IEnumerable<BoundNode> GetChildren()
    {
        yield return Condition;
        yield return ThenStatement;
        if (ElseStatement is not null)
            yield return ElseStatement;
    }
}