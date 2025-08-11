namespace Compiler.Binding;

public sealed class BoundBadExpression : BoundExpression
{
    public override BoundKind Kind => BoundKind.BadExpression;
    public override Type Type => typeof(object);
}