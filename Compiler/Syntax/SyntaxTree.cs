namespace Compiler.Syntax;
public sealed class SyntaxTree(IEnumerable<string> diagnostics, StatementSyntax root, SyntaxToken endOfFileToken) {
    public IReadOnlyList<string> Diagnostics { get; } = [.. diagnostics];
    public StatementSyntax Root { get; } = root;
    public SyntaxToken EndOfFileToken { get; } = endOfFileToken;
    public static SyntaxTree Parse(string text) {
        var parser = new Parser(text);
        return parser.Parse();
    }
}
