namespace Compiler.Binding;

public sealed class BoundBadStatement : BoundStatement
{
    public override BoundKind Kind => BoundKind.BadStatement;
    public override IEnumerable<BoundNode> GetChildren() => [];
}