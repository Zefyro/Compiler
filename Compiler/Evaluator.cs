namespace Compiler;
public sealed class Evaluator(ExpressionSyntax root)
{
    private readonly ExpressionSyntax _root = root;
    public object Evaluate() => EvaluateExpression(_root);

    private static object EvaluateExpression(ExpressionSyntax node)
    {
        if (node is LiteralExpressionSyntax n)
            return (double)n.LiteralToken.Value!;
        
        if (node is UnaryExpressionSyntax u)
        {
            object operand = EvaluateExpression(u.Operand);
            
            if (u.OperatorToken.Kind is SyntaxKind.PlusToken)
                return operand;
            else if (u.OperatorToken.Kind is SyntaxKind.MinusToken)
                return -(double)operand;
            else
                throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}");
        }
        
        if (node is BinaryExpressionSyntax b)
        {
            object left = EvaluateExpression(b.Left);
            object right = EvaluateExpression(b.Right);

            return b.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => (double)left + (double)right,
                SyntaxKind.MinusToken => (double)left - (double)right,
                SyntaxKind.StarToken => (double)left * (double)right,
                SyntaxKind.SlashToken => (double)left / (double)right,
                SyntaxKind.StarStarToken => (double)Math.Pow((double)left, (double)right),
                _ => throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}"),
            };
        }

        if (node is BooleanExpressionSyntax bo)
        {
            object left = EvaluateExpression(bo.Left);
            object right = EvaluateExpression(bo.Right);

            return bo.OperationToken.Kind switch {
                SyntaxKind.EqualsEqualsToken => (double)left == (double)right,
                SyntaxKind.LessthanToken => (double)left < (double)right,
                SyntaxKind.MorethanToken => (double)left > (double)right,
                SyntaxKind.LessthanEqualsToken => (double)left <= (double)right,
                SyntaxKind.MorethanEqualsToken => (double)left >= (double)right,
                _ => throw new Exception($"Unexpected boolean operator {bo.OperationToken.Kind}")
            };
        }
        
        if (node is ParenthesizedExpressionSyntax p)
            return EvaluateExpression(p.Expression);
        
        if (node is AssignmentExpressionSyntax a)
            return EvaluateExpression(a.Expression);

        throw new Exception($"Unexpected node {node.Kind}");
    }
}
