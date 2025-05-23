﻿namespace Compiler;
public sealed class SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken) {
    public IReadOnlyList<string> Diagnostics { get; } = [.. diagnostics];
    public ExpressionSyntax Root { get; } = root;
    public SyntaxToken EndOfFileToken { get; } = endOfFileToken;
    public static SyntaxTree Parse(string text) {
        var parser = new Parser(text);
        return parser.Parse();
    }
}
