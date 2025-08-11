using Compiler.Syntax;

namespace Compiler;
public sealed class Evaluator(ExpressionSyntax root) {
    private readonly ExpressionSyntax _root = root;
    public object Evaluate() => EvaluateExpression(_root);
    private static object EvaluateExpression(ExpressionSyntax node)
    {
        if (node is VariableExpressionSyntax v)
        {
            if (Program.Binder.Constants.TryGetValue(v.VariableToken.Text!, out double value))
            {
                return value;
            }

            Program.Binder.Variables.TryGetValue(v.VariableToken.Text!, out object? variable);
            if (variable is null)
            {
                Program.Diagnostics.Add($"'{v.VariableToken.Text!}' is not defined, it will be treated as '0'.");
                return 0.0d;
            }
            return variable;
        }

        if (node is LiteralExpressionSyntax n)
            return (double)n.LiteralToken.Value!;

        if (node is UnaryExpressionSyntax u)
        {
            double operand = (double)EvaluateExpression(u.Operand);

            if (u.OperatorToken.Kind is SyntaxKind.PlusToken)
                return operand;
            else if (u.OperatorToken.Kind is SyntaxKind.MinusToken)
                return -operand;
            else
                throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}");
        }

        if (node is BinaryExpressionSyntax b)
        {
            double left = (double)EvaluateExpression(b.Left);
            double right = (double)EvaluateExpression(b.Right);

            return b.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => left + right,
                SyntaxKind.MinusToken => left - right,
                SyntaxKind.StarToken => left * right,
                SyntaxKind.SlashToken => left / right,
                SyntaxKind.StarStarToken => Math.Pow(left, right),
                _ => throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}"),
            };
        }

        if (node is BooleanExpressionSyntax bo)
        {
            double left = (double)EvaluateExpression(bo.Left);
            double right = (double)EvaluateExpression(bo.Right);

            return bo.OperationToken.Kind switch
            {
                SyntaxKind.EqualsEqualsToken => left == right,
                SyntaxKind.LessthanToken => left < right,
                SyntaxKind.MorethanToken => left > right,
                SyntaxKind.LessthanEqualsToken => left <= right,
                SyntaxKind.MorethanEqualsToken => left >= right,
                _ => throw new Exception($"Unexpected boolean operator {bo.OperationToken.Kind}")
            };
        }

        if (node is ParenthesizedExpressionSyntax p)
            return EvaluateExpression(p.Expression);

        if (node is AssignmentExpressionSyntax a)
        {
            if (Program.Binder.Constants.TryGetValue(a.VariableToken.Text!, out double value))
            {
                Program.Diagnostics.Add($"Failed to bind expression '{EvaluateExpression(a.Expression)}' to a defined constant '{a.VariableToken.Text!}'");
                return value;
            }
            Program.Binder.BindExpression(a);
            return Program.Binder.Variables[a.VariableToken.Text!];
        }

        throw new Exception($"Unexpected node {node.Kind}");
    }
}
