namespace Compiler.Binding;

public sealed class BoundVariableExpression(string variableName, Type type) : BoundExpression
{
    public override BoundKind Kind => BoundKind.VariableExpression;
    public string VariableName { get; } = variableName;
    public override Type Type { get; } = type;
}