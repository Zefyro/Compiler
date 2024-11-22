using Lexer;

namespace Evaluator;
public sealed class Evaluator(ExpressionSyntax root)
{
    private readonly ExpressionSyntax _root = root;
    public object Evaluate() => EvaluateExpression(_root);

    private static object EvaluateExpression(ExpressionSyntax node)
    {
        if (node is LiteralExpressionSyntax n)
            return (float)n.LiteralToken.Value!;
        
        if (node is UnaryExpressionSyntax u)
        {
            object operand = EvaluateExpression(u.Operand);
            
            if (u.OperatorToken.Kind is SyntaxKind.PlusToken)
                return operand;
            else if (u.OperatorToken.Kind is SyntaxKind.MinusToken)
                return -(float)operand;
            else
                throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}");
        }
        
        if (node is BinaryExpressionSyntax b)
        {
            object left = EvaluateExpression(b.Left);
            object right = EvaluateExpression(b.Right);

            return b.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => (float)left + (float)right,
                SyntaxKind.MinusToken => (float)left - (float)right,
                SyntaxKind.StarToken => (float)left * (float)right,
                SyntaxKind.SlashToken => (float)left / (float)right,
                SyntaxKind.StarStarToken => (float)Math.Pow((float)left, (float)right),
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
