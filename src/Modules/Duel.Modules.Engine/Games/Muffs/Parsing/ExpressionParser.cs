using System.Globalization;
using Duel.Core.Games.Muffs.AST;
using Duel.Core.Games.Muffs.Tokenizing;
using Duel.Shared.Common;

namespace Duel.Core.Games.Muffs.Parsing;

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

    private static (Operator.Code code, int precedence, bool rightAssociative) GetOperatorMetadata(TokenType kind)
    {
        var code = kind switch
        {
            TokenType.Plus => Operator.Code.Add,
            TokenType.Minus => Operator.Code.Subtract,
            TokenType.Multiply => Operator.Code.Multiply,
            TokenType.Divide => Operator.Code.Divide,
            TokenType.Power => Operator.Code.Power,
            _ => Situation.Unreachable<Operator.Code>()
        };

        return (code, Operator.Precedence(code), Operator.IsRightAssociative(code));
    }

    private static bool IsBinaryOperator(TokenType type)
    {
        return type is TokenType.Plus or TokenType.Minus or TokenType.Multiply or TokenType.Divide or TokenType.Power;
    }

    private static Expression MakeBinary(Operator.Code code, Expression left, Expression right)
    {
        return code switch
        {
            Operator.Code.Add => Addition.From(left, right),
            Operator.Code.Subtract => Subtraction.From(left, right),
            Operator.Code.Multiply => Multiplication.From(left, right),
            Operator.Code.Divide => Division.From(left, right),
            Operator.Code.Power => Power.From(left, right),
            _ => Situation.Unreachable<Expression>()
        };
    }
}