namespace Compiler.Binding;

public enum BoundKind
{
    BadExpression,
    LiteralExpression,
    UnaryExpression,
    BinaryExpression,
    VariableExpression,
    AssignmentExpression,

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