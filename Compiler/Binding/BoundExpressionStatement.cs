namespace Compiler.Binding;

public sealed class BoundExpressionStatement(BoundExpression expression) : BoundStatement
{
    public override BoundKind Kind => BoundKind.ExpressionStatement;
    public BoundExpression Expression { get; } = expression;

    public override IEnumerable<BoundNode> GetChildren()
    {
        yield return Expression;
    }
}