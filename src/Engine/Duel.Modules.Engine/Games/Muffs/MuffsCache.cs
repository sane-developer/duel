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

    private readonly FrozenDictionary<int, ExpressionComposition[]> _compositions;

    public MuffsCache(ExpressionSettings settings)
    {
        var numbers = new List<Number>();
        
        var divisors = new List<(int, int[])>();
        
        var compositions = new List<ExpressionComposition>();

        for (var i = settings.Number.Start.Value; i <= settings.Number.End.Value; i++)
        {
            numbers.Add(new Number(i));

            if (settings.Divisions.IsAllowed) 
            {
                divisors.Add((i, i.Divisors().ToArray()));
            }

            if (settings.SquareRoots.IsAllowed && i % i is 0)
            {
                compositions.Add(new ExpressionComposition(new SquareRoot(new Number(i)), (int) Math.Sqrt(i)));
            }

            if (settings.Factorials.IsAllowed && i >= 0)
            {
                compositions.Add(new ExpressionComposition(new Factorial(new Number(i)), i.Factorial()));
            }

            if (settings.AbsoluteValues.IsAllowed)
            {
                compositions.Add(new ExpressionComposition(new Absolute(new Number(i)), Math.Abs(i)));
            }

            if (settings.Negations.IsAllowed)
            {
                compositions.Add(new ExpressionComposition(new Negate(new Number(i)), -i));
            }

            for (var j = settings.Number.Start.Value; j <= settings.Number.End.Value; j++)
            {
                if (settings.Additions.IsAllowed)
                {
                    compositions.Add(new ExpressionComposition(new Add(new Number(i), new Number(j)), i + j));
                }

                if (settings.Subtractions.IsAllowed)
                {
                    compositions.Add(new ExpressionComposition(new Subtract(new Number(i), new Number(j)), i - j));
                }
                
                if (settings.Multiplications.IsAllowed)
                {
                    compositions.Add(new ExpressionComposition(new Multiply(new Number(i), new Number(j)), i * j));
                }

                if (settings.Divisions.IsAllowed && j != 0)
                {
                    compositions.Add(new ExpressionComposition(new Divide(new Number(i), new Number(j)), i / j));
                }

                if (settings.Powers.IsAllowed)
                {
                    compositions.Add(new ExpressionComposition(new Power(new Number(i), new Number(j)), (int) Math.Pow(i, j)));
                }
                
                if (settings.Modulos.IsAllowed)
                {
                    compositions.Add(new ExpressionComposition(new Modulo(new Number(i), new Number(j)), i % j));
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

    public ExpressionComposition[] GetCompositions(int value) => _compositions[value];
}

public record ExpressionComposition(Symbol Expression, int Result);