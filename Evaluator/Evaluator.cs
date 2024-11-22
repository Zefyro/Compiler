using Lexer;

namespace Evaluator;
public sealed class Evaluator(ExpressionSyntax root)
{
    private readonly ExpressionSyntax _root = root;
    public object Evaluate() => EvaluateExpression(_root);

    private static object EvaluateExpression(ExpressionSyntax node)
    {
        if (node is LiteralExpressionSyntax n)
            return (int)n.LiteralToken.Value!;
        
        if (node is UnaryExpressionSyntax u)
        {
            object operand = EvaluateExpression(u.Operand);
            
            if (u.OperatorToken.Kind is SyntaxKind.PlusToken)
                return operand;
            else if (u.OperatorToken.Kind is SyntaxKind.MinusToken)
                return -(int)operand;
            else
                throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}");
        }
        
        if (node is BinaryExpressionSyntax b)
        {
            object left = EvaluateExpression(b.Left);
            object right = EvaluateExpression(b.Right);

            return b.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => (int)left + (int)right,
                SyntaxKind.MinusToken => (int)left - (int)right,
                SyntaxKind.StarToken => (int)left * (int)right,
                SyntaxKind.SlashToken => (int)left / (int)right,
                SyntaxKind.CaretToken => Math.Pow((int)left, (int)right),
                _ => throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}"),
            };
        }
        
        if (node is ParenthesizedExpressionSyntax p)
            return EvaluateExpression(p.Expression);
        
        if (node is AssignmentExpressionSyntax a)
            return EvaluateExpression(a.Expression);

        throw new Exception($"Unexpected node {node.Kind}");
    }
}
