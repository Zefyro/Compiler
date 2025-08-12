using Compiler.Binding;
using Compiler.Diagnostics;

namespace Compiler;
public sealed class Evaluator(BoundStatement root, DiagnosticBag diagnostics, Dictionary<string, object> variables)
{
    private readonly BoundStatement _root = root;
    private readonly DiagnosticBag _diagnostics = diagnostics;
    private readonly Dictionary<string, object> _variables = variables;

    public object Evaluate()
    {
        return EvaluateStatement(_root);
    }

    private object EvaluateStatement(BoundStatement node)
    {
        switch (node.Kind)
        {
            case BoundKind.BlockStatement:
                return EvaluateBlockStatement((BoundBlockStatement)node);
            case BoundKind.ExpressionStatement:
                return EvaluateExpression(((BoundExpressionStatement)node).Expression);
            case BoundKind.IfStatement:
                return EvaluateIfStatement((BoundIfStatement)node);
            case BoundKind.BadStatement:
                _diagnostics.Report("Received a BadStatement");
                return 0.0d;
            default:
                _diagnostics.Report($"Unexpected node {node.Kind}");
                return 0.0d;
        }
    }

    private object EvaluateBlockStatement(BoundBlockStatement node)
    {
        object? lastResult = null;
        foreach (var statement in node.Statements)
        {
            lastResult = EvaluateStatement(statement);
        }
        return lastResult!;
    }

    private object EvaluateIfStatement(BoundIfStatement node)
    {
        var condition = (bool)EvaluateExpression(node.Condition);
        if (condition)
        {
            return EvaluateStatement(node.ThenStatement);
        }
        else if (node.ElseStatement is not null)
        {
            return EvaluateStatement(node.ElseStatement);
        }
        return 0.0d; // TODO: smarter null handling
    }

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