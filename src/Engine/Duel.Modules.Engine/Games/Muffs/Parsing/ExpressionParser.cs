using System.Globalization;
using Duel.Modules.Engine.Games.Muffs.AST;
using Duel.Modules.Engine.Games.Muffs.Tokenizing;

namespace Duel.Modules.Engine.Games.Muffs.Parsing;

public sealed class ExpressionParser(List<ExpressionToken> tokens)
{
    private readonly ExpressionParserState _state = new(tokens);
    
    public Expression Parse()
    {
        var expression = ParseExpression(minimumPrecedence: 0);

        _state.Expect(TokenType.EndOfInput);
        
        return expression;
    }

    private Expression ParseExpression(int minimumPrecedence)
    {
        var left = ParsePrimary();

        while (true)
        {
            var next = _state.Current;

            if (!IsBinaryOperator(next.Type))
            {
                break;
            }

            var (opCode, precedence, rightAssociative) = GetOperatorMetadata(next.Type);

            if (precedence < minimumPrecedence)
            {
                break;
            }

            _state.Advance();

            var nextMin = rightAssociative ? precedence : precedence + 1;
            
            var right = ParseExpression(nextMin);

            left = MakeBinary(opCode, left, right);
        }

        return left;
    }

    private Expression ParsePrimary()
    {
        var token = _state.Current;

        if (token.Type is TokenType.UnaryMinus or TokenType.Abs or TokenType.Sqrt or TokenType.Factorial)
        {
            return ParseUnaryFunction(token.Type);
        }

        if (token.Type is TokenType.Number)
        {
            _state.Advance();

            if (!int.TryParse(token.Value, NumberStyles.None, CultureInfo.InvariantCulture, out var value))
            {
                throw new FormatException($"Invalid number literal '{token.Value}'.");
            }

            return Constant.From(value);
        }

        if (token.Type is TokenType.LeftParen)
        {
            _state.Advance();
            
            var inner = ParseExpression(minimumPrecedence: 0);
            
            _state.Expect(TokenType.RightParen);
            
            return inner;
        }

        throw new FormatException($"Unexpected token '{token.Value}'.");
    }

    private Expression ParseUnaryFunction(TokenType type)
    {
        _state.Advance();
        
        _state.Expect(TokenType.LeftParen);
        
        var operand = ParseExpression(minimumPrecedence: 0);
        
        _state.Expect(TokenType.RightParen);
        
        return type switch
        {
            TokenType.UnaryMinus => Negation.From(operand),
            TokenType.Sqrt => SquareRoot.From(operand),
            TokenType.Abs => Abs.From(operand),
            TokenType.Factorial => Factorial.From(operand),
            _ => Situation.Unreachable<Expression>()
        };
    }

    private static (Binary.Type type, int precedence, bool rightAssociative) GetOperatorMetadata(TokenType kind)
    {
        var code = kind switch
        {
            TokenType.Plus => Binary.Type.Add,
            TokenType.Minus => Binary.Type.Subtract,
            TokenType.Multiply => Binary.Type.Multiply,
            TokenType.Divide => Binary.Type.Divide,
            TokenType.Modulo => Binary.Type.Modulo,
            TokenType.Power => Binary.Type.Power,
            _ => Situation.Unreachable<Binary.Type>()
        };

        return (code, Binary.Precedence(code), Binary.IsRightAssociative(code));
    }

    private static bool IsBinaryOperator(TokenType type)
    {
        return type is TokenType.Plus or TokenType.Minus or TokenType.Multiply or TokenType.Divide or TokenType.Modulo or TokenType.Power;
    }

    private static Expression MakeBinary(Binary.Type type, Expression lhs, Expression rhs)
    {
        return type switch
        {
            Binary.Type.Add => Addition.From(lhs, rhs),
            Binary.Type.Subtract => Subtraction.From(lhs, rhs),
            Binary.Type.Multiply => Multiplication.From(lhs, rhs),
            Binary.Type.Divide => Division.From(lhs, rhs),
            Binary.Type.Modulo => Modulo.From(lhs, rhs),
            Binary.Type.Power => Power.From(lhs, rhs),
            _ => Situation.Unreachable<Expression>()
        };
    }
}