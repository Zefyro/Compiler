namespace Compiler.Syntax;
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
    OpenBraceToken,
    CloseBraceToken,
    //PlusEqualsToken,
    //MinusEqualsToken,
    //StarEqualsToken,
    //SlashEqualsToken,
    LessthanToken,
    MorethanToken,
    EqualsEqualsToken,
    BangEqualsToken,
    LessthanEqualsToken,
    MorethanEqualsToken,
    EndOfExpressionToken,
    VariableToken,

    // Keywords
    ElseKeyword,
    IfKeyword,
    FalseKeyword,
    TrueKeyword,

    // Nodes
    BlockStatement,
    ElseClause,
    IfStatement,

    // Expressions
    AssignmentExpression,
    BinaryExpression,
    UnaryExpression,
    ParenthesizedExpression,
    LiteralExpression,
    VariableExpression,
    ExpressionStatement,
}