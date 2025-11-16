using Duel.Modules.Engine.Muffs.Glyphs;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Duel.Modules.Engine.Muffs.Compositions;

public sealed class NumberRegistry(int minimum, int maximum)
{    
    private readonly FrozenDictionary<int, Number> _numbers = Enumerable
        .Range(minimum, count: maximum - minimum + 1)
        .ToFrozenDictionary(n => n, Number.From);

    public Number GetNumber(int value)
    {
        return _numbers.GetValueOrDefault(value, Number.From(value));
    }
}

public sealed class CompositionRegistry(List<Composition> compositions)
{
    private readonly FrozenDictionary<CompositionRegistryKey, ImmutableArray<Composition>> _compositions = compositions
        .Where(CompositionRegistryFilter.IsValid)
        .GroupBy(CompositionRegistryKey.From)
        .ToFrozenDictionary(g => g.Key, g => g.ToImmutableArray());

    public ImmutableArray<Composition> GetCompositions(int result, GlyphType operatorType)
    {
        var key = CompositionRegistryKey.From(result, operatorType);

        return _compositions.TryGetValue(key, out var compositions) ? compositions : ImmutableArray<Composition>.Empty;
    }

    private readonly record struct CompositionRegistryKey(int Result, GlyphType OperatorType)
    {
        public static CompositionRegistryKey From(int result, GlyphType operatorType)
        {
            return new CompositionRegistryKey(result, operatorType);
        }

        public static CompositionRegistryKey From(Composition composition)
        {
            return new CompositionRegistryKey(composition.Result, composition.OperatorType);
        }
    }
}

file static class CompositionRegistryFilter
{
    private static readonly List<ICompositionFilter> _filters =
    [
        new PerfectSquareFilter(), new PerfectDivisorFilter(), new NonZeroResultFilter()
    ];

    public static bool IsValid(Composition composition)
    {
        return _filters.All(restriction => restriction.IsSatisfied(composition));
    }
}

file sealed class PerfectSquareFilter : ICompositionFilter
{
    public bool IsSatisfied(Composition composition)
    {
        if (composition.OperatorType is not GlyphType.SquareRoot)
        {
            return true;
        }

        if (composition is not UnaryComposition unary)
        {
            return Situation.Unreachable<bool>();
        }

        var result = (int) Math.Sqrt(unary.Operand);

        return result * result == unary.Operand;
    }
}

file sealed class PerfectDivisorFilter : ICompositionFilter
{
    public bool IsSatisfied(Composition composition)
    {
        if (composition.OperatorType is not GlyphType.Divide)
        {
            return true;
        }

        return composition is BinaryComposition binary && binary.Lhs % binary.Rhs == 0;
    }
}

file sealed class NonZeroResultFilter : ICompositionFilter
{
    public bool IsSatisfied(Composition composition)
    {
        return composition.Result != 0;
    }
}

file interface ICompositionFilter
{
    bool IsSatisfied(Composition composition);
}