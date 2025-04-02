namespace Compiler;
public static class SyntaxFacts {
    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind) {
        return kind switch {
            SyntaxKind.StarStarToken => 4,
            SyntaxKind.StarToken or SyntaxKind.SlashToken => 3,
            SyntaxKind.PlusToken or SyntaxKind.MinusToken => 2,
            SyntaxKind.EqualsEqualsToken or SyntaxKind.LessthanToken or SyntaxKind.MorethanToken 
            or SyntaxKind.LessthanEqualsToken or SyntaxKind.MorethanEqualsToken => 1,
            _ => 0,
        };
    }
    public static int GetUnaryOperatorPrecedence(this SyntaxKind kind) {
        return kind switch {
            SyntaxKind.PlusToken or SyntaxKind.MinusToken => 5,
            _ => 0,
        };
    }
}
