namespace Compiler.Binding;

public sealed class BoundAssignmentExpression(string variableName, BoundExpression expression, Type type) : BoundExpression
{
    public override BoundKind Kind => BoundKind.AssignmentExpression;
    public string VariableName { get; } = variableName;
    public BoundExpression Expression { get; } = expression;
    public override Type Type { get; } = type;

    public override IEnumerable<BoundNode> GetChildren()
    {
        yield return Expression;
    }
}