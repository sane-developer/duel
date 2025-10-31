using Duel.Modules.Engine.Games.Muffs.Generation.Operators;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Compositions;

public static class CompositionBuilder
{
    public static IEnumerable<Composition> For(OperatorType type, IntegerRange result)
    {
        return type switch
        {
            OperatorType.Addition => FromAddition(result),
            OperatorType.Subtraction => FromSubtraction(result),
            OperatorType.Multiplication => FromMultiplication(result),
            OperatorType.Division => FromDivision(result),
            OperatorType.Modulo => FromModulo(result),
            OperatorType.Power => FromPower(result),
            OperatorType.Negation => FromNegation(result),
            OperatorType.Absolute => FromAbsolute(result),
            OperatorType.Factorial => FromFactorial(result),
            OperatorType.SquareRoot => FromSquareRoot(result),
            _ => Situation.Unreachable<IEnumerable<Composition>>(),
        };
    }

    private static IEnumerable<BinaryComposition> FromAddition(IntegerRange result)
    {
        for (var x = -200; x <= 200; x++)
        {
            for (var y = -200; y <= 200; y++)
            {
                var output = x + y;

                if (output >= result.Minimum && output <= result.Maximum)
                {
                    yield return new BinaryComposition(OperatorType.Addition, x, y, output);
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromSubtraction(IntegerRange result)
    {
        for (var x = -200; x <= 200; x++)
        {
            for (var y = -200; y <= 200; y++)
            {
                var output = x - y;

                if (output >= result.Minimum && output <= result.Maximum)
                {
                    yield return new BinaryComposition(OperatorType.Subtraction, x, y, output);
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromMultiplication(IntegerRange result)
    {
        for (var x = -30; x <= 30; x++)
        {
            for (var y = -30; y <= 30; y++)
            {
                var output = x * y;

                if (output >= result.Minimum && output <= result.Maximum)
                {
                    yield return new BinaryComposition(OperatorType.Multiplication, x, y, output);
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromDivision(IntegerRange result)
    {
        for (var x = -200; x <= 200; x++)
        {
            for (var y = -2; y <= 15; y++)
            {
                if (x.IsDivisibleBy(y))
                {
                    var output = x / y;

                    if (output >= result.Minimum && output <= result.Maximum)
                    {
                        yield return new BinaryComposition(OperatorType.Division, x, y, output);
                    }
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromModulo(IntegerRange result)
    {
        for (var x = -200; x <= 200; x++)
        {
            for (var y = 2; y <= 25; y++)
            {
                var output = x % y;

                if (output >= result.Minimum && output <= result.Maximum)
                {
                    yield return new BinaryComposition(OperatorType.Modulo, x, y, output);
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromPower(IntegerRange result)
    {
        for (var lhs = -12; lhs <= 12; lhs++)
        {
            for (var rhs = 2; rhs <= 4; rhs++)
            {
                if (lhs.IsSafePower(rhs))
                {
                    var output = (int) Math.Pow(lhs, rhs);

                    if (output >= result.Minimum && output <= result.Maximum)
                    {
                        yield return new BinaryComposition(OperatorType.Power, lhs, rhs, output);
                    }
                }
            }
        }
    }

    private static IEnumerable<UnaryComposition> FromNegation(IntegerRange result)
    {
        for (var x = -200; x <= 200; x++)
        {
            var output = -x;

            if (output >= result.Minimum && output <= result.Maximum)
            {
                yield return new UnaryComposition(OperatorType.Negation, x, output);
            }
        }
    }

    private static IEnumerable<UnaryComposition> FromAbsolute(IntegerRange result)
    {
        for (var x = -200; x <= 200; x++)
        {
            var output = Math.Abs(x);
            
            if (output >= result.Minimum && output <= result.Maximum)
            {
                yield return new UnaryComposition(OperatorType.Absolute, x, output);
            }
        }
    }

    private static IEnumerable<UnaryComposition> FromFactorial(IntegerRange result)
    {
        for (var operand = 0; operand <= 6; operand++)
        {
            var output = operand.Factorial();

            if (output >= result.Minimum && output <= result.Maximum)
            {
                yield return new UnaryComposition(OperatorType.Factorial, operand, output);
            }
        }
    }

    private static IEnumerable<UnaryComposition> FromSquareRoot(IntegerRange result)
    {
        var maximum = result.Maximum * result.Maximum;
        
        for (var x = 0; x <= maximum; x++)
        {
            if (x.IsPerfectSquare())
            {
                var output = (int) Math.Sqrt(x);

                if (output >= result.Minimum && output <= result.Maximum)
                {
                    yield return new UnaryComposition(OperatorType.SquareRoot, x, output);
                }
            }
        }
    }
}