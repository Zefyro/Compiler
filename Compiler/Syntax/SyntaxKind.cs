namespace Compiler;
public enum SyntaxKind {
    // Tokens
    InvalidToken,
    EndOfFileToken,
    WhitespaceToken,
    NumberToken,
    CloseParenthesisToken,
    OpenParenthesisToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    StarStarToken,
    SlashSlashToken,
    SlashStarToken,
    EqualsToken,
    LessthanToken,
    MorethanToken,
    EqualsEqualsToken,
    LessthanEqualsToken,
    MorethanEqualsToken,
    EndOfExpressionToken,
    VariableToken,

    // Expressions
    AssignmentExpression,
    BinaryExpression,
    UnaryExpression,
    ParenthesizedExpression,
    LiteralExpression,
    VariableExpression,
}
