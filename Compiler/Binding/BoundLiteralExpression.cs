namespace Compiler.Binding;

public sealed class BoundLiteralExpression(object value) : BoundExpression
{
    public override BoundKind Kind => BoundKind.LiteralExpression;
    public object Value { get; } = value;
    public override Type Type { get; } = value.GetType();
}