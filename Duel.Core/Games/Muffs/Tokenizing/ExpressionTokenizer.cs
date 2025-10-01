using Duel.Core.Shared;

namespace Duel.Core.Games.Muffs.Tokenizing;

public sealed class ExpressionTokenizer(string text)
{
    public List<ExpressionToken> Tokenize()
    {
        var tokens = new List<ExpressionToken>();
        
        var cursorIndex = 0;

        while (cursorIndex < text.Length)
        {
            cursorIndex = GetNextNonWhitespaceCharacterIndex(text, cursorIndex);

            if (cursorIndex >= text.Length)
            {
                break;
            }

            var character = text[cursorIndex];

            if (char.IsDigit(character))
            {
                var (numberToken, nextCharacterIndex) = GetNumberToken(text, cursorIndex);

                cursorIndex = nextCharacterIndex;

                tokens.Add(numberToken);

                continue;
            }

            var operatorToken = GetOperatorToken(character);
            
            tokens.Add(operatorToken);

            cursorIndex++;
        }

        tokens.Add(ExpressionTokenRegistry.EndOfInput);
        
        return tokens;
    }

    private static int GetNextNonWhitespaceCharacterIndex(string expression, int startIndex)
    {
        var cursorIndex = startIndex;
        
        while (cursorIndex < expression.Length)
        {
            var character = expression[cursorIndex];

            if (!char.IsWhiteSpace(character))
            {
                break;
            }
            
            cursorIndex++;
        }

        return cursorIndex;
    }

    private static (ExpressionToken token, int nextCharacterIndex) GetNumberToken(string expression, int startIndex)
    {
        var cursorIndex = startIndex;
        
        while (cursorIndex < expression.Length)
        {
            var character = expression[cursorIndex];

            if (!char.IsDigit(character))
            {
                break;
            }
            
            cursorIndex++;
        }

        var slice = expression[startIndex..cursorIndex];
        
        var token = ExpressionToken.From(TokenType.Number, slice);
        
        return (token, cursorIndex);
    }
    
    private static ExpressionToken GetOperatorToken(char character)
    {
        return character switch
        {
            '+' => ExpressionTokenRegistry.Plus,
            '-' => ExpressionTokenRegistry.Minus,
            '*' => ExpressionTokenRegistry.Multiply,
            '/' => ExpressionTokenRegistry.Divide,
            '^' => ExpressionTokenRegistry.Power,
            '(' => ExpressionTokenRegistry.LeftParen,
            ')' => ExpressionTokenRegistry.RightParen,
            _ => Situation.Unreachable<ExpressionToken>()
        };
    }
}