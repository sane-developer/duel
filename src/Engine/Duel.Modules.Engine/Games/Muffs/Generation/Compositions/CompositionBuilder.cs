using Duel.Modules.Engine.Games.Muffs.Generation.Operators;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Compositions;

public static class CompositionBuilder
{
    public static IEnumerable<Composition> From(OperatorType type, int minResult, int maxResult)
    {
        return type switch
        {
            OperatorType.Addition => FromAddition(minResult, maxResult),
            OperatorType.Subtraction => FromSubtraction(minResult, maxResult),
            OperatorType.Multiplication => FromMultiplication(minResult, maxResult),
            OperatorType.Division => FromDivision(minResult, maxResult),
            OperatorType.Modulo => FromModulo(minResult, maxResult),
            OperatorType.Power => FromPower(minResult, maxResult),
            OperatorType.Negation => FromNegation(minResult, maxResult),
            OperatorType.Absolute => FromAbsolute(minResult, maxResult),
            OperatorType.Factorial => FromFactorial(minResult, maxResult),
            OperatorType.SquareRoot => FromSquareRoot(minResult, maxResult),
            _ => Situation.Unreachable<IEnumerable<Composition>>(),
        };
    }

    private static IEnumerable<BinaryComposition> FromAddition(int minResult, int maxResult)
    {
        var searchMin = Math.Min(minResult, minResult / 2);

        var searchMax = maxResult;
        
        for (var lhs = searchMin; lhs <= searchMax; lhs++)
        {
            for (var rhs = searchMin; rhs <= searchMax; rhs++)
            {
                var result = lhs + rhs;

                if (result >= minResult && result <= maxResult)
                {
                    yield return new BinaryComposition(OperatorType.Addition, lhs, rhs, result);
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromSubtraction(int minResult, int maxResult)
    {
        var searchMin = minResult;

        var searchMax = maxResult + Math.Abs(minResult);
        
        for (var lhs = searchMin; lhs <= searchMax; lhs++)
        {
            for (var rhs = 0; rhs <= searchMax - searchMin; rhs++)
            {
                var result = lhs - rhs;

                if (result >= minResult && result <= maxResult)
                {
                    yield return new BinaryComposition(OperatorType.Subtraction, lhs, rhs, result);
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromMultiplication(int minResult, int maxResult)
    {
        var searchMin = Math.Min(minResult, 0);

        var searchMax = Math.Max(maxResult, Math.Abs(minResult));
        
        for (var lhs = searchMin; lhs <= searchMax; lhs++)
        {
            for (var rhs = searchMin; rhs <= searchMax; rhs++)
            {
                var result = lhs * rhs;

                if (result >= minResult && result <= maxResult)
                {
                    yield return new BinaryComposition(OperatorType.Multiplication, lhs, rhs, result);
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromDivision(int minResult, int maxResult)
    {
        var searchMax = Math.Max(Math.Abs(minResult), Math.Abs(maxResult)) * 10;

        var searchMin = -searchMax;
        
        for (var lhs = searchMin; lhs <= searchMax; lhs++)
        {
            for (var rhs = -maxResult; rhs <= maxResult; rhs++)
            {
                if (lhs.IsDivisibleBy(rhs))
                {
                    var result = lhs / rhs;

                    if (result >= minResult && result <= maxResult)
                    {
                        yield return new BinaryComposition(OperatorType.Division, lhs, rhs, result);
                    }
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromModulo(int minResult, int maxResult)
    {
        var searchMax = Math.Max(maxResult * 2, 100);
        
        for (var lhs = 0; lhs <= searchMax; lhs++)
        {
            for (var rhs = 1; rhs <= searchMax; rhs++)
            {
                var result = lhs % rhs;
                
                if (result >= minResult && result <= maxResult)
                {
                    yield return new BinaryComposition(OperatorType.Modulo, lhs, rhs, result);
                }
            }
        }
    }

    private static IEnumerable<BinaryComposition> FromPower(int minResult, int maxResult)
    {
        var baseMax = (int) Math.Ceiling(Math.Pow(maxResult, 0.5)) + 5;
        
        var baseMin = -baseMax;
        
        for (var lhs = baseMin; lhs <= baseMax; lhs++)
        {
            for (var rhs = 0; rhs <= 10; rhs++)
            {
                if (lhs.IsSafePower(rhs))
                {
                    var result = (int)Math.Pow(lhs, rhs);

                    if (result >= minResult && result <= maxResult)
                    {
                        yield return new BinaryComposition(OperatorType.Power, lhs, rhs, result);
                    }
                }
            }
        }
    }

    private static IEnumerable<UnaryComposition> FromNegation(int minResult, int maxResult)
    {
        for (var operand = -maxResult; operand <= -minResult; operand++)
        {
            var result = -operand;
            
            if (result >= minResult && result <= maxResult)
            {
                yield return new UnaryComposition(OperatorType.Negation, operand, result);
            }
        }
    }

    private static IEnumerable<UnaryComposition> FromAbsolute(int minResult, int maxResult)
    {
        for (var operand = -maxResult; operand <= maxResult; operand++)
        {
            var result = Math.Abs(operand);
            
            if (result >= minResult && result <= maxResult)
            {
                yield return new UnaryComposition(OperatorType.Absolute, operand, result);
            }
        }
    }

    private static IEnumerable<UnaryComposition> FromFactorial(int minResult, int maxResult)
    {
        const int maxSafeFactorial = 12;
        
        for (var operand = 0; operand <= maxSafeFactorial; operand++)
        {
            var result = operand.Factorial();
            
            if (result >= minResult && result <= maxResult)
            {
                yield return new UnaryComposition(OperatorType.Factorial, operand, result);
            }
            
            if (result > maxResult)
            {
                break;
            }
        }
    }

    private static IEnumerable<UnaryComposition> FromSquareRoot(int minResult, int maxResult)
    {
        var operandMin = minResult * minResult;

        var operandMax = maxResult * maxResult;
        
        for (var operand = operandMin; operand <= operandMax; operand++)
        {
            if (operand.IsPerfectSquare())
            {
                var result = (int) Math.Sqrt(operand);
                
                if (result >= minResult && result <= maxResult)
                {
                    yield return new UnaryComposition(OperatorType.SquareRoot, operand, result);
                }
            }
        }
    }
}

