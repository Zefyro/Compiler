using Compiler.Syntax;

namespace Compiler.Binding;

public sealed class BoundUnaryOperator(SyntaxKind syntaxKind, BoundKind kind, Type operandType, Type resultType)
{
    public SyntaxKind SyntaxKind { get; } = syntaxKind;
    public BoundKind Kind { get; } = kind;
    public Type OperandType { get; } = operandType;
    public Type ResultType { get; } = resultType;

    private static readonly BoundUnaryOperator[] _operators = [
        new(SyntaxKind.PlusToken, BoundKind.UnaryPlusExpression, typeof(double), typeof(double)),
        new(SyntaxKind.MinusToken, BoundKind.UnaryMinusExpression, typeof(double), typeof(double)),
    ];

    public static BoundUnaryOperator? Bind(SyntaxKind syntaxKind, Type operandType)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.OperandType == operandType)
                return op;
        }
        return null;
    }
}