using Duel.Modules.Engine.Games.Muffs.Expressions;
using Duel.Modules.Engine.Games.Muffs.Glyphs;
using Duel.Modules.Engine.Games.Muffs.Glyphs.Literals;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs;

public sealed class MuffsCache
{
    private readonly FrozenDictionary<int, int[]> _divisors;

    private readonly FrozenDictionary<int, Number> _numbers;

    private readonly FrozenDictionary<int, Composition[]> _compositions;

    public MuffsCache(ExpressionSettings settings)
    {
        var uniqueNumbers = new HashSet<int>();

        var divisors = new List<(int, int[])>();

        var compositions = new List<Composition>();

        BuildBinaryCompositions(settings.Addition, OperatorType.Addition, (a, b) => a + b, uniqueNumbers, compositions, divisors);
        
        BuildBinaryCompositions(settings.Subtraction, OperatorType.Subtraction, (a, b) => a - b, uniqueNumbers, compositions, divisors);
        
        BuildBinaryCompositions(settings.Multiplication, OperatorType.Multiplication, (a, b) => a * b, uniqueNumbers, compositions, divisors);
        
        BuildBinaryCompositions(settings.Division, OperatorType.Division, (a, b) => a / b, uniqueNumbers, compositions, divisors, requireNonZeroRhs: true);
        
        BuildBinaryCompositions(settings.Modulo, OperatorType.Modulo, (a, b) => a % b, uniqueNumbers, compositions, divisors, requireNonZeroRhs: true);
        
        BuildBinaryCompositions(settings.Power, OperatorType.Power, (a, b) => (int) Math.Pow(a, b), uniqueNumbers, compositions, divisors, validator: IsSafePower);

        BuildUnaryCompositions(settings.Negation, OperatorType.Negation, a => -a, uniqueNumbers, compositions);

        BuildUnaryCompositions(settings.AbsoluteValue, OperatorType.AbsoluteValue, Math.Abs, uniqueNumbers, compositions);

        BuildUnaryCompositions(settings.Factorial, OperatorType.Factorial, a => a.Factorial(), uniqueNumbers, compositions, validator: a => a >= 0 && a <= 5);

        BuildUnaryCompositions(settings.SquareRoot, OperatorType.SquareRoot, a => (int) Math.Sqrt(a), uniqueNumbers, compositions, validator: IsPerfectSquare);

        _numbers = uniqueNumbers
            .ToDictionary(n => n, n => new Number(n))
            .ToFrozenDictionary();

        _divisors = divisors
            .ToDictionary(d => d.Item1, d => d.Item2)
            .ToFrozenDictionary();

        _compositions = compositions
            .GroupBy(c => c.Result)
            .ToDictionary(g => g.Key, g => g.ToArray())
            .ToFrozenDictionary();
    }

    private static void BuildBinaryCompositions(
        OperatorSettings settings,
        OperatorType type,
        Func<int, int, int> operation,
        HashSet<int> numbers,
        List<Composition> compositions,
        List<(int, int[])> divisors,
        bool requireNonZeroRhs = false,
        Func<int, int, bool>? validator = null)
    {
        if (!settings.IsAllowed) return;

        var range = settings.Operand;

        for (var i = range.Start; i <= range.End; i++)
        {
            numbers.Add(i);
            
            if (type == OperatorType.Division && !divisors.Any(d => d.Item1 == i))
            {
                divisors.Add((i, i.Divisors().ToArray()));
            }

            for (var j = range.Start; j <= range.End; j++)
            {
                if (requireNonZeroRhs && j == 0) 
                {
                    continue;
                }
                
                if (validator != null && !validator(i, j)) 
                {
                    continue;
                }

                try
                {
                    var result = operation(i, j);
                    
                    if (settings.Result.HasValue)
                    {
                        var resultRange = settings.Result.Value;
                        
                        if (result < resultRange.Start || result > resultRange.End)
                        {
                            continue;
                        }
                    }

                    compositions.Add(new BinaryComposition(type, i, j, result));

                    numbers.Add(result);
                }
                catch
                {
                    // Skip if operation fails (overflow, etc.)
                }
            }
        }
    }

    private static void BuildUnaryCompositions(
        OperatorSettings settings,
        OperatorType type,
        Func<int, int> operation,
        HashSet<int> numbers,
        List<Composition> compositions,
        Func<int, bool>? validator = null)
    {
        if (!settings.IsAllowed) 
        {
            return;
        }

        var range = settings.Operand;

        for (var i = range.Start; i <= range.End; i++)
        {
            if (validator != null && !validator(i)) 
            {
                continue;
            }

            try
            {
                var result = operation(i);
                
                if (settings.Result.HasValue)
                {
                    var resultRange = settings.Result.Value;
                    
                    if (result < resultRange.Start || result > resultRange.End)
                    {
                        continue;
                    }
                }

                compositions.Add(new UnaryComposition(type, i, result));
                
                numbers.Add(i);
                
                numbers.Add(result);
            }
            catch
            {
                // Skip if operation fails
            }
        }
    }

    public Number GetNumber(int value)
    {
        return _numbers[value];
    }

    public int[] GetDivisors(int value)
    {
        return _divisors[value];
    }

    public Composition[] GetCompositions(int value)
    {
        return _compositions[value];
    }

    private static bool IsPerfectSquare(int value)
    {
        if (value < 0) 
        {
            return false;
        }

        return Math.Sqrt(value) % 1 is 0;
    }

    private static bool IsSafePower(int @base, int exponent)
    {
        if (exponent < 0) 
        {
            return false;
        }

        if (exponent is 0) 
        {
            return true;
        }

        if (@base is 0 or 1 or -1) 
        {
            return true;
        }

        try
        {
            return Math.Pow(@base, exponent) is >= int.MinValue and <= int.MaxValue;
        }
        catch
        {
            return false;
        }
    }
}

public abstract record Composition(OperatorType Type, int Result);

public sealed record UnaryComposition(OperatorType Type, int Operand, int Result) : Composition(Type, Result);

public sealed record BinaryComposition(OperatorType Type, int Lhs, int Rhs, int Result) : Composition(Type, Result);