using Compiler.Binding;
using Compiler.Diagnostics;

namespace Compiler;
public sealed class Evaluator(BoundExpression root, DiagnosticBag diagnostics, Dictionary<string, object> variables)
{
    private readonly BoundExpression _root = root;
    private readonly DiagnosticBag _diagnostics = diagnostics;
    private readonly Dictionary<string, object> _variables = variables;

    public object Evaluate() => EvaluateExpression(_root);
    private object EvaluateExpression(BoundExpression node)
    {
        if (node is BoundLiteralExpression n)
            return n.Value;

        if (node is BoundUnaryExpression u)
        {
            object operand = EvaluateExpression(u.Operand);

            switch (u.OperatorKind)
            {
                case BoundKind.UnaryPlusExpression:
                    return (double)operand;
                case BoundKind.UnaryMinusExpression:
                    return -(double)operand;
                default:
                    _diagnostics.Report($"Unexpected unary operator {u.OperatorKind}");
                    return 0.0d;
            }
        }

        if (node is BoundBinaryExpression b)
        {
            object left = EvaluateExpression(b.Left);
            object right = EvaluateExpression(b.Right);

            switch (b.OperatorKind)
            {
                case BoundKind.BinaryAdditionExpression:
                    return (double)left + (double)right;
                case BoundKind.BinarySubtractionExpression:
                    return (double)left - (double)right;
                case BoundKind.BinaryMultiplicationExpression:
                    return (double)left * (double)right;
                case BoundKind.BinaryDivisionExpression:
                    return (double)left / (double)right;
                case BoundKind.BinaryPowerExpression:
                    return Math.Pow((double)left, (double)right);
                case BoundKind.BinaryEqualsExpression:
                    return Equals(left, right);
                case BoundKind.BinaryNotEqualsExpression:
                    return !Equals(left, right);
                case BoundKind.BinaryLessThanExpression:
                    return (double)left < (double)right;
                case BoundKind.BinaryGreaterThanExpression:
                    return (double)left > (double)right;
                case BoundKind.BinaryLessThanEqualsExpression:
                    return (double)left <= (double)right;
                case BoundKind.BinaryGreaterThanEqualsExpression:
                    return (double)left >= (double)right;
                default:
                    _diagnostics.Report($"Unexpected binary operator {b.OperatorKind}");
                    return 0.0d;
            }
        }

        if (node is BoundVariableExpression v)
        {
            if (_variables.TryGetValue(v.VariableName, out object? value))
                return value;
            else
            {
                _diagnostics.ReportUndefinedVariable(v.VariableName);
                return 0.0d;
            }
        }

        if (node is BoundAssignmentExpression a)
        {
            object value = EvaluateExpression(a.Expression);
            _variables[a.VariableName] = value;
            return value;
        }

        _diagnostics.Report($"Unexpected node {node.Kind}");
        return 0.0d;
    }
}