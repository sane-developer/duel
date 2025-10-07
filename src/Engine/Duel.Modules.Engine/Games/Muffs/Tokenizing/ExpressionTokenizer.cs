namespace Duel.Modules.Engine.Games.Muffs.Tokenizing;

public sealed class ExpressionTokenizer(string text)
{
    public List<ExpressionToken> Tokenize()
    {
        var tokens = new List<ExpressionToken>();
        
        var cursor = 0;

        while (cursor < text.Length)
        {
            cursor = GetNextNonWhitespaceCharacterIndex(text, cursor);

            if (cursor >= text.Length)
            {
                break;
            }

            var character = text[cursor];

            if (char.IsDigit(character))
            {
                var (numberToken, nextCharacterIndex) = GetNumberToken(text, cursor);

                cursor = nextCharacterIndex;

                tokens.Add(numberToken);

                continue;
            }

            if (char.IsLetter(character))
            {
                var (identifierToken, nextCharacterIndex) = GetIdentifierToken(text, cursor);

                cursor = nextCharacterIndex;

                tokens.Add(identifierToken);

                continue;
            }

            var operatorToken = GetOperatorToken(character);
            
            tokens.Add(operatorToken);

            cursor++;
        }

        tokens.Add(ExpressionTokenRegistry.EndOfInput);
        
        return tokens;
    }

    private static int GetNextNonWhitespaceCharacterIndex(string expression, int startIndex)
    {
        var cursor = startIndex;
        
        while (cursor < expression.Length)
        {
            var character = expression[cursor];

            if (!char.IsWhiteSpace(character))
            {
                break;
            }
            
            cursor++;
        }

        return cursor;
    }

    private static (ExpressionToken token, int nextCharacterIndex) GetNumberToken(string expression, int startIndex)
    {
        var cursor = startIndex;
        
        while (cursor < expression.Length)
        {
            var character = expression[cursor];

            if (!char.IsDigit(character))
            {
                break;
            }
            
            cursor++;
        }

        var slice = expression[startIndex..cursor];
        
        var token = ExpressionToken.From(TokenType.Number, slice);
        
        return (token, cursor);
    }
    
    private static (ExpressionToken token, int nextCharacterIndex) GetIdentifierToken(string expression, int startIndex)
    {
        var cursor = startIndex;
        
        while (cursor < expression.Length)
        {
            var character = expression[cursor];

            if (!char.IsLetterOrDigit(character))
            {
                break;
            }
            
            cursor++;
        }

        var identifier = expression[startIndex..cursor];

        var type = identifier switch
        {
            "minus" => TokenType.UnaryMinus,
            "abs" => TokenType.Abs,
            "sqrt" => TokenType.Sqrt,
            "factorial" => TokenType.Factorial,
            _ => throw new FormatException($"Unknown function: {identifier}")
        };
        
        var token = ExpressionToken.From(type, identifier);
        
        return (token, cursor);
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
            '%' => ExpressionTokenRegistry.Modulo,
            '(' => ExpressionTokenRegistry.LeftParen,
            ')' => ExpressionTokenRegistry.RightParen,
            _ => Situation.Unreachable<ExpressionToken>()
        };
    }
}