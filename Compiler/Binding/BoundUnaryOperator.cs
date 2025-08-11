using Compiler.Syntax;

namespace Compiler.Binding;

public sealed class BoundUnaryOperator
{
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundKind kind, Type operandType, Type resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
        ResultType = resultType;
    }

    public SyntaxKind SyntaxKind { get; }
    public BoundKind Kind { get; }
    public Type OperandType { get; }
    public Type ResultType { get; }

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