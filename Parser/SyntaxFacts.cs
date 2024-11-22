using Lexer;

namespace Parser;
public static class SyntaxFacts
{
    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind) 
	{
		return kind switch
		{
			SyntaxKind.StarStarToken => 3,
			SyntaxKind.StarToken or SyntaxKind.SlashToken => 2,
			SyntaxKind.PlusToken or SyntaxKind.MinusToken => 1,
			_ => 0,
		};
	}
    public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
    {
		return kind switch
		{
			SyntaxKind.PlusToken or SyntaxKind.MinusToken => 3,
			_ => 0,
		};
	}
}
