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
    CaretToken,
    SlashSlashToken,
    SlashStarToken,
    EqualsToken,
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
