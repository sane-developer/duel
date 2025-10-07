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

        _state.Expect(ExpressionTokenType.EndOfInput);
        
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

        if (token.Type is ExpressionTokenType.UnaryMinus or ExpressionTokenType.Abs or ExpressionTokenType.Sqrt or ExpressionTokenType.Factorial)
        {
            return ParseUnaryFunction(token.Type);
        }

        if (token.Type is ExpressionTokenType.Number)
        {
            _state.Advance();

            if (!int.TryParse(token.Value, NumberStyles.None, CultureInfo.InvariantCulture, out var value))
            {
                throw new FormatException($"Invalid number literal '{token.Value}'.");
            }

            return Constant.From(value);
        }

        if (token.Type is ExpressionTokenType.LeftParen)
        {
            _state.Advance();
            
            var inner = ParseExpression(minimumPrecedence: 0);
            
            _state.Expect(ExpressionTokenType.RightParen);
            
            return inner;
        }

        throw new FormatException($"Unexpected token '{token.Value}'.");
    }

    private Expression ParseUnaryFunction(ExpressionTokenType type)
    {
        _state.Advance();
        
        _state.Expect(ExpressionTokenType.LeftParen);
        
        var operand = ParseExpression(minimumPrecedence: 0);
        
        _state.Expect(ExpressionTokenType.RightParen);
        
        return type switch
        {
            ExpressionTokenType.UnaryMinus => Negation.From(operand),
            ExpressionTokenType.Sqrt => SquareRoot.From(operand),
            ExpressionTokenType.Abs => Abs.From(operand),
            ExpressionTokenType.Factorial => Factorial.From(operand),
            _ => Situation.Unreachable<Expression>()
        };
    }

    private static (Binary.Type type, int precedence, bool rightAssociative) GetOperatorMetadata(ExpressionTokenType kind)
    {
        var code = kind switch
        {
            ExpressionTokenType.Plus => Binary.Type.Add,
            ExpressionTokenType.Minus => Binary.Type.Subtract,
            ExpressionTokenType.Multiply => Binary.Type.Multiply,
            ExpressionTokenType.Divide => Binary.Type.Divide,
            ExpressionTokenType.Modulo => Binary.Type.Modulo,
            ExpressionTokenType.Power => Binary.Type.Power,
            _ => Situation.Unreachable<Binary.Type>()
        };

        return (code, Binary.Precedence(code), Binary.IsRightAssociative(code));
    }

    private static bool IsBinaryOperator(ExpressionTokenType type)
    {
        return type is ExpressionTokenType.Plus or ExpressionTokenType.Minus or ExpressionTokenType.Multiply or ExpressionTokenType.Divide or ExpressionTokenType.Modulo or ExpressionTokenType.Power;
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