using Compiler.Syntax;
using System.Collections.Immutable;

namespace Compiler;
public sealed class Parser {
    private readonly SyntaxToken[] _tokens;
    private int _position;
    private List<string> _diagnostics = [];
    public Parser(string text) {
        List<SyntaxToken> tokens = [];
        Lexer lexer = new(text);
        SyntaxToken token;
        do {
            token = lexer.NextToken();
            if (token.Kind is not SyntaxKind.WhitespaceToken
                && token.Kind is not SyntaxKind.InvalidToken
                && token.Kind is not SyntaxKind.SlashSlashToken
                && token.Kind is not SyntaxKind.SlashStarToken)
            {
                tokens.Add(token);
            }

        } while (token.Kind is not SyntaxKind.EndOfFileToken);
        _tokens = [.. tokens];
    }
    private SyntaxToken Peek(int offset) {
        int index = _position + offset;
        if (index >= _tokens.Length)
            return _tokens[^1];
        return _tokens[index];
    }
    private SyntaxToken Current => Peek(0);
    private SyntaxToken Next() {
        SyntaxToken current = Current;
        _position++;
        return current;
    }
    private SyntaxToken Match(SyntaxKind kind) {
        if (Current.Kind == kind)
            return Next();
        _diagnostics.Add($"Unexpected token <{Current.Kind}>, expected <{kind}>");
        return new SyntaxToken(kind, Current.Position, null, null);
    }
    public SyntaxTree Parse() {
        StatementSyntax statement = ParseStatement();
        SyntaxToken endOfFileToken = Match(SyntaxKind.EndOfFileToken);
        return new SyntaxTree(_diagnostics, statement, endOfFileToken);
    }

    private StatementSyntax ParseStatement()
    {
        switch (Current.Kind)
        {
            case SyntaxKind.OpenBraceToken:
                return ParseBlockStatement();
            case SyntaxKind.IfKeyword:
                return ParseIfStatement();
            default:
                return ParseExpressionStatement();
        }
    }

    private StatementSyntax ParseExpressionStatement()
    {
        var expression = ParseExpression();
        return new ExpressionStatementSyntax(expression);
    }

    private StatementSyntax ParseBlockStatement()
    {
        var openBraceToken = Match(SyntaxKind.OpenBraceToken);
        var statements = new List<StatementSyntax>();

        while (Current.Kind != SyntaxKind.CloseBraceToken &&
               Current.Kind != SyntaxKind.EndOfFileToken)
        {
            var statement = ParseStatement();
            statements.Add(statement);
        }

        var closeBraceToken = Match(SyntaxKind.CloseBraceToken);
        return new BlockStatementSyntax(openBraceToken, statements.ToImmutableArray(), closeBraceToken);
    }

    private StatementSyntax ParseIfStatement()
    {
        var ifKeyword = Match(SyntaxKind.IfKeyword);
        var openParenthesisToken = Match(SyntaxKind.OpenParenthesisToken);
        var condition = ParseExpression();
        var closeParenthesisToken = Match(SyntaxKind.CloseParenthesisToken);
        var thenStatement = ParseStatement();
        var elseClause = ParseElseClause();

        return new IfStatementSyntax(ifKeyword, condition, thenStatement, elseClause);
    }

    private ElseClauseSyntax? ParseElseClause()
    {
        if (Current.Kind == SyntaxKind.ElseKeyword)
        {
            var elseKeyword = Next();
            var elseStatement = ParseStatement();
            return new ElseClauseSyntax(elseKeyword, elseStatement);
        }
        return null;
    }

    private ExpressionSyntax ParseExpression() {
        return ParseAssignmentExpression();
    }
    private ExpressionSyntax ParseAssignmentExpression() {
        if (Current.Kind == SyntaxKind.VariableToken &&
            Peek(1).Kind == SyntaxKind.EqualsToken) {
            SyntaxToken variableToken = Next();
            SyntaxToken operatorToken = Next();
            ExpressionSyntax right = ParseBinaryExpression();
            return new AssignmentExpressionSyntax(variableToken, operatorToken, right);
        }

        return ParseBinaryExpression();
    }
    private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0) {
        ExpressionSyntax left;
        int unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence) {
            SyntaxToken operatorToken = Next();
            ExpressionSyntax operand = ParseBinaryExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }
        else {
            left = ParsePrimaryExpression();
        }
        while (true) {
            int precedence = Current.Kind.GetBinaryOperatorPrecedence();
            if (precedence == 0 || precedence <= parentPrecedence)
                break;
            
            SyntaxToken operatorToken = Next();
            ExpressionSyntax right = ParseBinaryExpression(precedence);
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }
        return left;
    }
    private ExpressionSyntax ParsePrimaryExpression() {
        switch (Current.Kind) {
            case SyntaxKind.OpenParenthesisToken:
                SyntaxToken left = Next();
                ExpressionSyntax expression = ParseBinaryExpression();
                SyntaxToken right = Match(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);

            case SyntaxKind.VariableToken:
                SyntaxToken variableToken = Next();
                return new VariableExpressionSyntax(variableToken);

            case SyntaxKind.TrueKeyword:
            case SyntaxKind.FalseKeyword:
                SyntaxToken booleanToken = Next();
                return new LiteralExpressionSyntax(booleanToken);

            default:
                SyntaxToken literalToken = Match(SyntaxKind.NumberToken);
                return new LiteralExpressionSyntax(literalToken);
        }
    }
}