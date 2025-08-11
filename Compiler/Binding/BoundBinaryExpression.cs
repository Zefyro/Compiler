namespace Compiler.Binding;

public sealed class BoundBinaryExpression(BoundExpression left, BoundKind operatorKind, BoundExpression right, Type type) : BoundExpression
{
    public override BoundKind Kind => BoundKind.BinaryExpression;
    public BoundExpression Left { get; } = left;
    public BoundKind OperatorKind { get; } = operatorKind;
    public BoundExpression Right { get; } = right;
    public override Type Type { get; } = type;

    public override IEnumerable<BoundNode> GetChildren()
    {
        yield return Left;
        yield return Right;
    }
}