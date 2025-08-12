using Compiler.Syntax;

namespace Compiler.Tests;

public class LexerTests
{
    [Theory]
    [InlineData("1", SyntaxKind.NumberToken, 1.0d)]
    [InlineData("123", SyntaxKind.NumberToken, 123.0d)]
    [InlineData("1.23", SyntaxKind.NumberToken, 1.23d)]
    [InlineData("1.2.3", SyntaxKind.NumberToken, 1.2d)] // Lexer should stop at second decimal
    [InlineData("+", SyntaxKind.PlusToken, null)]
    [InlineData("-", SyntaxKind.MinusToken, null)]
    [InlineData("*", SyntaxKind.StarToken, null)]
    [InlineData("/", SyntaxKind.SlashToken, null)]
    [InlineData("**", SyntaxKind.StarStarToken, null)]
    [InlineData("(", SyntaxKind.OpenParenthesisToken, null)]
    [InlineData(")", SyntaxKind.CloseParenthesisToken, null)]
    [InlineData("=", SyntaxKind.EqualsToken, null)]
    [InlineData("==", SyntaxKind.EqualsEqualsToken, null)]
    [InlineData("!=", SyntaxKind.BangEqualsToken, null)]
    [InlineData("<", SyntaxKind.LessthanToken, null)]
    [InlineData(">", SyntaxKind.MorethanToken, null)]
    [InlineData("<=", SyntaxKind.LessthanEqualsToken, null)]
    [InlineData(">=", SyntaxKind.MorethanEqualsToken, null)]
    [InlineData(";", SyntaxKind.EndOfExpressionToken, null)]
    [InlineData("abc", SyntaxKind.VariableToken, "abc")]
    [InlineData("_var", SyntaxKind.VariableToken, "_var")]
    [InlineData("true", SyntaxKind.TrueKeyword, true)]
    [InlineData("false", SyntaxKind.FalseKeyword, false)]
    [InlineData("  ", SyntaxKind.WhitespaceToken, null)]
    [InlineData("// comment", SyntaxKind.SlashSlashToken, null)]
    [InlineData("/* comment */", SyntaxKind.SlashStarToken, null)]
    public void Lexer_Lexes_Token(string text, SyntaxKind expectedKind, object? expectedValue)
    {
        // Arrange
        var lexer = new Lexer(text);

        // Act
        var token = lexer.NextToken();

        // Assert
        Assert.Equal(expectedKind, token.Kind);
        Assert.Equal(expectedValue, token.Value);
    }

    [Fact]
    public void Lexer_Lexes_AllTokens()
    {
        // Arrange
        var text = "1 + 2 * (3 - 4) == 5 != 6 <= 7 >= 8 ; // comment\n/* multi\nline */ var_name = 10 ??";
        var expectedTokens = new (SyntaxKind Kind, object? Value)[]
        {
            (SyntaxKind.NumberToken, 1.0d),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.PlusToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.NumberToken, 2.0d),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.StarToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.OpenParenthesisToken, null),
            (SyntaxKind.NumberToken, 3.0d),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.MinusToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.NumberToken, 4.0d),
            (SyntaxKind.CloseParenthesisToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.EqualsEqualsToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.NumberToken, 5.0d),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.BangEqualsToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.NumberToken, 6.0d),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.LessthanEqualsToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.NumberToken, 7.0d),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.MorethanEqualsToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.NumberToken, 8.0d),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.EndOfExpressionToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.SlashSlashToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.SlashStarToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.VariableToken, "var_name"),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.EqualsToken, null),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.NumberToken, 10.0d),
            (SyntaxKind.WhitespaceToken, null),
            (SyntaxKind.InvalidToken, null),
            (SyntaxKind.InvalidToken, null),
            (SyntaxKind.EndOfFileToken, null)
        };

        // Act
        var tokens = new List<SyntaxToken>();
        var lexer = new Lexer(text);
        SyntaxToken token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Kind != SyntaxKind.EndOfFileToken);

        // Assert
        Assert.Equal(expectedTokens.Length, tokens.Count);
        for (int i = 0; i < expectedTokens.Length; i++)
        {
            Assert.Equal(expectedTokens[i].Kind, tokens[i].Kind);
            Assert.Equal(expectedTokens[i].Value, tokens[i].Value);
        }
    }
}