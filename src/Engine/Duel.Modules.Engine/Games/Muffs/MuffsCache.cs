using Duel.Modules.Engine.Games.Muffs.AST;
using Duel.Modules.Engine.Games.Muffs.AST.Literals;
using Duel.Modules.Engine.Games.Muffs.AST.Operators;
using Duel.Shared.Extensions;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs;

public sealed class MuffsCache
{
    private readonly FrozenDictionary<int, int[]> _divisors;

    private readonly FrozenDictionary<int, Number> _numbers;

    private readonly FrozenDictionary<int, Composition[]> _compositions;

    public MuffsCache(ExpressionSettings settings)
    {
        var numbers = new List<Number>();
        
        var divisors = new List<(int, int[])>();
        
        var compositions = new List<Composition>();

        for (var i = settings.Number.Start.Value; i <= settings.Number.End.Value; i++)
        {
            numbers.Add(new Number(i));

            if (settings.Division.IsAllowed) 
            {
                divisors.Add((i, i.Divisors().ToArray()));
            }

            if (settings.SquareRoot.IsAllowed && i % i is 0)
            {
                compositions.Add(new Composition(new SquareRoot(new Number(i)), (int) Math.Sqrt(i)));
            }

            if (settings.Factorial.IsAllowed && i >= 0)
            {
                compositions.Add(new Composition(new Factorial(new Number(i)), i.Factorial()));
            }

            if (settings.AbsoluteValue.IsAllowed)
            {
                compositions.Add(new Composition(new Absolute(new Number(i)), Math.Abs(i)));
            }

            if (settings.Negation.IsAllowed)
            {
                compositions.Add(new Composition(new Negate(new Number(i)), -i));
            }

            for (var j = settings.Number.Start.Value; j <= settings.Number.End.Value; j++)
            {
                if (settings.Addition.IsAllowed)
                {
                    compositions.Add(new Composition(new Add(new Number(i), new Number(j)), i + j));
                }

                if (settings.Subtraction.IsAllowed)
                {
                    compositions.Add(new Composition(new Subtract(new Number(i), new Number(j)), i - j));
                }
                
                if (settings.Multiplication.IsAllowed)
                {
                    compositions.Add(new Composition(new Multiply(new Number(i), new Number(j)), i * j));
                }

                if (settings.Division.IsAllowed && j != 0)
                {
                    compositions.Add(new Composition(new Divide(new Number(i), new Number(j)), i / j));
                }

                if (settings.Power.IsAllowed)
                {
                    compositions.Add(new Composition(new Power(new Number(i), new Number(j)), (int) Math.Pow(i, j)));
                }
                
                if (settings.Modulo.IsAllowed)
                {
                    compositions.Add(new Composition(new Modulo(new Number(i), new Number(j)), i % j));
                }
            }
        }

        _numbers = numbers
            .ToDictionary(number => number.Value, number => number)
            .ToFrozenDictionary();

        _divisors = divisors
            .ToDictionary(divisor => divisor.Item1, divisor => divisor.Item2)
            .ToFrozenDictionary();

        _compositions = compositions
            .GroupBy(composition => composition.Result)
            .ToDictionary(group => group.Key, group => group.ToArray())
            .ToFrozenDictionary();
    }

    public Number GetNumber(int value) => _numbers[value];

    public int[] GetDivisors(int value) => _divisors[value];

    public Composition[] GetCompositions(int value) => _compositions[value];
}

public readonly record struct Composition(Glyph Expression, int Result);