using Compiler.Syntax;
using System.Collections.Generic;

namespace Compiler.Binding;

public sealed class BoundUnaryExpression(BoundKind operatorKind, BoundExpression operand, Type type) : BoundExpression
{
    public override BoundKind Kind => BoundKind.UnaryExpression;
    public BoundKind OperatorKind { get; } = operatorKind;
    public BoundExpression Operand { get; } = operand;
    public override Type Type { get; } = type;

    public override IEnumerable<BoundNode> GetChildren()
    {
        yield return Operand;
    }
}