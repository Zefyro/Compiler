namespace Lexer;
public enum SyntaxKind
{
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
    SlashSlashToken,
    SlashStarToken,
    EqualsToken,
    VariableToken,

    // Expressions
    AssignmentExpression,
    BinaryExpression,
    UnaryExpression,
    ParenthesizedExpression,
    LiteralExpression,
    VariableExpression,
}
