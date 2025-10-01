using Duel.Core.Games.Muffs.Tokenizing;

namespace Duel.Core.Games.Muffs.Parsing;

public sealed class ExpressionParserState(List<ExpressionToken> tokens)
{
    private int _position;
    
    public ExpressionToken Current => CanGetToken() ? GetToken() : ExpressionTokenRegistry.EndOfInput;

    public void Advance()
    {
        _position++;
    }

    public bool Match(TokenType type)
    {
        if (Current.Type != type)
        {
            return false;
        }
        
        Advance();
        
        return true;
    }

    public void Expect(TokenType type)
    {
        if (Current.Type != type)
        {
            throw new FormatException($"Expected {type}, found '{Current.Value}'.");
        }

        Advance();
    }

    private bool CanGetToken()
    {
        return _position < tokens.Count;
    }
    
    private ExpressionToken GetToken()
    {
        return tokens[_position];
    }
}