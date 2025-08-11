using Compiler.Syntax;

namespace Compiler.Binding;

public sealed class BoundBinaryOperator(SyntaxKind syntaxKind, BoundKind kind, Type leftType, Type rightType, Type resultType)
{
    public SyntaxKind SyntaxKind { get; } = syntaxKind;
    public BoundKind Kind { get; } = kind;
    public Type LeftType { get; } = leftType;
    public Type RightType { get; } = rightType;
    public Type ResultType { get; } = resultType;

    private static readonly BoundBinaryOperator[] _operators = [
        new(SyntaxKind.PlusToken, BoundKind.BinaryAdditionExpression, typeof(double), typeof(double), typeof(double)),
        new(SyntaxKind.MinusToken, BoundKind.BinarySubtractionExpression, typeof(double), typeof(double), typeof(double)),
        new(SyntaxKind.StarToken, BoundKind.BinaryMultiplicationExpression, typeof(double), typeof(double), typeof(double)),
        new(SyntaxKind.SlashToken, BoundKind.BinaryDivisionExpression, typeof(double), typeof(double), typeof(double)),
        new(SyntaxKind.StarStarToken, BoundKind.BinaryPowerExpression, typeof(double), typeof(double), typeof(double)),

        new(SyntaxKind.EqualsEqualsToken, BoundKind.BinaryEqualsExpression, typeof(double), typeof(double), typeof(bool)),
        new(SyntaxKind.LessthanToken, BoundKind.BinaryLessThanExpression, typeof(double), typeof(double), typeof(bool)),
        new(SyntaxKind.MorethanToken, BoundKind.BinaryGreaterThanExpression, typeof(double), typeof(double), typeof(bool)),
        new(SyntaxKind.LessthanEqualsToken, BoundKind.BinaryLessThanEqualsExpression, typeof(double), typeof(double), typeof(bool)),
        new(SyntaxKind.MorethanEqualsToken, BoundKind.BinaryGreaterThanEqualsExpression, typeof(double), typeof(double), typeof(bool)),

        new(SyntaxKind.EqualsEqualsToken, BoundKind.BinaryEqualsExpression, typeof(bool), typeof(bool), typeof(bool)),
        new(SyntaxKind.BangEqualsToken, BoundKind.BinaryNotEqualsExpression, typeof(bool), typeof(bool), typeof(bool)),
    ];

    public static BoundBinaryOperator? Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType)
                return op;
        }
        return null;
    }
}