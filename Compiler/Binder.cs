using Compiler.Binding;
using Compiler.Diagnostics;
using Compiler.Syntax;
using System.Collections.Immutable;

namespace Compiler;

public sealed class Binder(DiagnosticBag diagnostics, Dictionary<string, object> variables)
{
    private readonly DiagnosticBag _diagnostics = diagnostics;
    private readonly Dictionary<string, object> _variables = variables;

    public BoundStatement BindStatement(StatementSyntax syntax)
    {
        switch (syntax.Kind)
        {
            case SyntaxKind.BlockStatement:
                return BindBlockStatement((BlockStatementSyntax)syntax);
            case SyntaxKind.ExpressionStatement:
                return BindExpressionStatement((ExpressionStatementSyntax)syntax);
            case SyntaxKind.IfStatement:
                return BindIfStatement((IfStatementSyntax)syntax);
            default:
                _diagnostics.Report($"Unexpected syntax {syntax.Kind}");
                return new BoundBadStatement();
        }
    }

    public BoundExpression BindExpression(ExpressionSyntax syntax)
    {
        switch (syntax.Kind)
        {
            case SyntaxKind.LiteralExpression:
                return BindLiteralExpression((LiteralExpressionSyntax)syntax);
            case SyntaxKind.BinaryExpression:
                return BindBinaryExpression((BinaryExpressionSyntax)syntax);
            case SyntaxKind.ParenthesizedExpression:
                return BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);
            case SyntaxKind.UnaryExpression:
                return BindUnaryExpression((UnaryExpressionSyntax)syntax);
            case SyntaxKind.VariableExpression:
                return BindVariableExpression((VariableExpressionSyntax)syntax);
            case SyntaxKind.AssignmentExpression:
                return BindAssignmentExpression((AssignmentExpressionSyntax)syntax);
            default:
                _diagnostics.Report($"Unexpected syntax {syntax.Kind}");
                return new BoundBadExpression();
        }
    }

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
    {
        var value = syntax.LiteralToken.Value;
        return new BoundLiteralExpression(value!);
    }

    private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
    {
        var boundLeft = BindExpression(syntax.Left);
        var boundRight = BindExpression(syntax.Right);
        var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

        if (boundOperator is null)
        {
            _diagnostics.ReportInvalidOperator(syntax.OperatorToken.Text!, boundLeft.Type, boundRight.Type);
            return new BoundBadExpression();
        }

        return new BoundBinaryExpression(boundLeft, boundOperator.Kind, boundRight, boundOperator.ResultType);
    }

    private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
    {
        return BindExpression(syntax.Expression);
    }

    private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
    {
        var boundOperand = BindExpression(syntax.Operand);
        var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

        if (boundOperator is null)
        {
            _diagnostics.ReportInvalidOperator(syntax.OperatorToken.Text!, boundOperand.Type, null);
            return new BoundBadExpression();
        }

        return new BoundUnaryExpression(boundOperator.Kind, boundOperand, boundOperator.ResultType);
    }

    private BoundExpression BindVariableExpression(VariableExpressionSyntax syntax)
    {
        if (!_variables.TryGetValue(syntax.VariableToken.Text!, out object? value))
        {
            _diagnostics.ReportUndefinedVariable(syntax.VariableToken.Text!);
            return new BoundBadExpression();
        }
        return new BoundVariableExpression(syntax.VariableToken.Text!, value.GetType());
    }

    private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
    {
        var boundExpression = BindExpression(syntax.Expression);
        return new BoundAssignmentExpression(syntax.VariableToken.Text!, boundExpression, boundExpression.Type);
    }

    private BoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
    {
        var boundExpression = BindExpression(syntax.Expression);
        return new BoundExpressionStatement(boundExpression);
    }

    private BoundStatement BindBlockStatement(BlockStatementSyntax syntax)
    {
        var statements = ImmutableArray.CreateBuilder<BoundStatement>();
        foreach (var statementSyntax in syntax.Statements)
        {
            var boundStatement = BindStatement(statementSyntax);
            statements.Add(boundStatement);
        }
        return new BoundBlockStatement(statements.ToImmutable());
    }

    private BoundStatement BindIfStatement(IfStatementSyntax syntax)
    {
        var boundCondition = BindExpression(syntax.Condition);
        var boundThenStatement = BindStatement(syntax.ThenStatement);
        var boundElseStatement = syntax.ElseClause is null ? null : BindStatement(syntax.ElseClause.ElseStatement);

        if (boundCondition.Type != typeof(bool))
        {
            _diagnostics.Report($"Condition of type '{boundCondition.Type}' cannot be converted to 'System.Boolean'");
        }

        return new BoundIfStatement(boundCondition, boundThenStatement, boundElseStatement);
    }
}