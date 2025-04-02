namespace Compiler;
public sealed class Parser
{
    private readonly SyntaxToken[] _tokens;
    private int _position;
	private List<string> _diagnostics = new();
    public Parser(string text)
    {
        List<SyntaxToken> tokens = new();
        Lexer lexer = new(text);
        SyntaxToken token;
        do
        {
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
    private SyntaxToken Peek(int offset)
    {
        int index = _position + offset;
        if (index >= _tokens.Length)
            return _tokens[^1];
        return _tokens[index];
    }
    private SyntaxToken Current => Peek(0);
    private SyntaxToken Next()
    {
        SyntaxToken current = Current;
        _position++;
        return current;
    }
    private SyntaxToken Match(SyntaxKind kind)
    {
        if (Current.Kind == kind)
            return Next();
        _diagnostics.Add($"Unexpected token <{Current.Kind}>, expected <{kind}>");
		return new SyntaxToken(kind, Current.Position, null, null);
    }
    public SyntaxTree Parse()
	{
		ExpressionSyntax expression = ParseExpression();
		SyntaxToken endOfFileToken = Match(SyntaxKind.EndOfFileToken);
		return new SyntaxTree(_diagnostics, expression, endOfFileToken);
	}
    private ExpressionSyntax ParseExpression()
    {
        return ParseAssignmentExpression();
    }
    private ExpressionSyntax ParseAssignmentExpression()
    {
        if (Current.Kind == SyntaxKind.VariableToken &&
            Peek(1).Kind == SyntaxKind.EqualsToken)
        {
            SyntaxToken variableToken = Next();
            SyntaxToken operatorToken = Next();
            ExpressionSyntax right = ParseBinaryExpression();
            return new AssignmentExpressionSyntax(variableToken, operatorToken, right);
        }

        return ParseBinaryExpression();
    }
    private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0) 
	{
		ExpressionSyntax left;
        int unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
		if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
        {
            SyntaxToken operatorToken = Next();
            ExpressionSyntax operand = ParseBinaryExpression(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }
        else
        {
            left = ParsePrimaryExpression();
        }
		while (true) 
		{
			int precedence = Current.Kind.GetBinaryOperatorPrecedence();
			if (precedence == 0 || precedence <= parentPrecedence)
				break;
			
			SyntaxToken operatorToken = Next();
			ExpressionSyntax right = ParseBinaryExpression(precedence);
            if (operatorToken.Kind is SyntaxKind.EqualsEqualsToken or SyntaxKind.LessthanToken or SyntaxKind.MorethanToken or SyntaxKind.LessthanEqualsToken or SyntaxKind.MorethanEqualsToken)
                left = new BooleanExpressionSyntax(left, operatorToken, right);
            else
                left = new BinaryExpressionSyntax(left, operatorToken, right);
		}
		return left;
	}
	private ExpressionSyntax ParsePrimaryExpression()
	{
        switch (Current.Kind)
        {
            case SyntaxKind.OpenParenthesisToken:
                SyntaxToken left = Next();
                ExpressionSyntax expression = ParseBinaryExpression();
                SyntaxToken right = Match(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);

            case SyntaxKind.VariableToken:
                SyntaxToken variableToken = Next();
                return new VariableExpressionSyntax(variableToken);

            default:
                SyntaxToken literalToken = Match(SyntaxKind.NumberToken);
                return new LiteralExpressionSyntax(literalToken);
        }
	}
}
