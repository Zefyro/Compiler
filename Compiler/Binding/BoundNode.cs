namespace Compiler.Binding;

public abstract class BoundNode
{
    public abstract BoundKind Kind { get; }

    public virtual IEnumerable<BoundNode> GetChildren() => [];
}