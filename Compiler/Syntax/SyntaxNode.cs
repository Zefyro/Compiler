﻿namespace Compiler;
public abstract class SyntaxNode {
    public abstract SyntaxKind Kind { get; }
    public abstract IEnumerable<SyntaxNode> GetChildren();
}
