﻿namespace Compiler;
public sealed class LiteralExpressionSyntax(SyntaxToken literalToken) : ExpressionSyntax {
    public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
    public SyntaxToken LiteralToken { get; } = literalToken;
    public override IEnumerable<SyntaxNode> GetChildren() {
        yield return LiteralToken;
    }
}
