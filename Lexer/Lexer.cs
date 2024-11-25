using System.Globalization;

namespace Lexer;
public sealed class Lexer(string text)
{
    private readonly string _text = text;
    private int _position;

    private char Peek(int offset)
    {
        int index = _position + offset;
        return index >= _text.Length ? '\0' : _text[index];
    }
    private char Current => Peek(0);
    private void Next()
    {
        _position++;
    }
    public SyntaxToken NextToken()
    {
        if (_position >= _text.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
        
        int start = _position;
        
        if (char.IsDigit(Current))
        {
            // FIXME: make sure to allow only 1 decimal point in a floating point value
            while (char.IsDigit(Current) || Current == '.')
                Next();
            
            int length = _position - start;
            string text = _text.Substring(start, length);
            _ = int.TryParse(text, out int iValue);
            bool isDouble = double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double dValue);
            object value = isDouble ? dValue : iValue;
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }
        else if (char.IsWhiteSpace(Current))
        {
            while (char.IsWhiteSpace(Current))
                Next();
            
            int length = _position - start;
            string text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
        }
        else if (Current == '_')
        {
            while (char.IsLetter(Current) || Current == '_')
                Next();
            
            int length = _position - start;
            string text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.VariableToken, start, text, text);
        }
        
        switch (Current)
        {
            case '+':
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            case '-':
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            case '*':
                if (Peek(1) == '*')
                    {
                        Next();
                        return new SyntaxToken(SyntaxKind.StarStarToken, _position++, "**", null);
                    }
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
            case '/':
                {
                    if (Peek(1) == '/')
                    {
                        while ((Current != '\n') && (Current != '\0'))
                            Next();
                        
                        int length = _position - start;
                        string text = _text.Substring(start, length);
                        return new SyntaxToken(SyntaxKind.SlashSlashToken, start, text, null);
                    }
                    else if (Peek(1) == '*')
                    {
                        while (!((Current == '*') && (Peek(1) == '/')) && (Current != '\0'))
                            Next();
                        
                        if (Current != '\0') Next();
                        var length = _position++ - start;
                        if (start + length > _text.Length) length--;
                        
                        var text = _text.Substring(start, length);
                        return new SyntaxToken(SyntaxKind.SlashStarToken, start, text, null);
                    }
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                }
            case '(':
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            case ')':
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
            case '=':
                {
                    if (Peek(1) == '=')
                    {
                        Next();
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position++, "==", null);
                    }
                    return new SyntaxToken(SyntaxKind.EqualsToken, _position++, "=", null);
                }
            case ';':
                return new SyntaxToken(SyntaxKind.EndOfExpressionToken, _position++, ";", null);
            case '<':
                {
                    if (Peek(1) == '=')
                    {
                        Next();
                        return new SyntaxToken(SyntaxKind.LessthanEqualsToken, _position++, "<=", null);
                    }
                    return new SyntaxToken(SyntaxKind.LessthanToken, _position++, "<", null);
                }
            case '>':
                {
                    if (Peek(1) == '=')
                    {
                        Next();
                        return new SyntaxToken(SyntaxKind.MorethanEqualsToken, _position++, ">=", null);
                    }
                    return new SyntaxToken(SyntaxKind.MorethanToken, _position++, ">", null);
                }
        }
        return new SyntaxToken(SyntaxKind.InvalidToken, _position++, _text.Substring(_position - 1, 1), null);
    }
}
