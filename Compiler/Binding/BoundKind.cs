namespace Compiler.Binding;

public enum BoundKind
{
    BadExpression,
    BadStatement,
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    VariableExpression,
    AssignmentExpression,
    BlockStatement,
    IfStatement,
    ExpressionStatement,

    // Unary Operators
    UnaryPlusExpression,
    UnaryMinusExpression,

    // Binary Operators
    BinaryAdditionExpression,
    BinarySubtractionExpression,
    BinaryMultiplicationExpression,
    BinaryDivisionExpression,
    BinaryPowerExpression,
    BinaryEqualsExpression,
    BinaryNotEqualsExpression,
    BinaryLessThanExpression,
    BinaryGreaterThanExpression,
    BinaryLessThanEqualsExpression,
    BinaryGreaterThanEqualsExpression,
}