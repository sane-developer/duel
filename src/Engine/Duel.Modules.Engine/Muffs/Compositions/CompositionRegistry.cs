using Duel.Modules.Engine.Muffs.Glyphs;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Muffs.Compositions;

public sealed class CompositionRegistry(List<Composition> compositions)
{
    private readonly FrozenSet<CompositionRegistryItem> _compositions = compositions
        .Where(CompositionRegistryPolicy.IsValid)
        .GroupBy(x => CompositionRegistryKey.From(x.Result, x.OperatorType))
        .Select(x => CompositionRegistryItem.From(x.Key.Result, x.Key.OperatorType, [.. x]))
        .ToFrozenSet();

    public List<Composition> For(int result, GlyphType operatorType)
    {
        return _compositions.First(x => x.Key.Result == result && x.Key.OperatorType == operatorType).Compositions;
    }

    private readonly record struct CompositionRegistryKey(int Result, GlyphType OperatorType)
    {
        public static CompositionRegistryKey From(int result, GlyphType operatorType)
        {
            return new CompositionRegistryKey(result, operatorType);
        }
    }

    private readonly record struct CompositionRegistryItem(CompositionRegistryKey Key, List<Composition> Compositions)
    {
        public static CompositionRegistryItem From(int result, GlyphType operatorType, List<Composition> compositions)
        {
            var key = CompositionRegistryKey.From(result, operatorType);

            return new CompositionRegistryItem(key, compositions);
        }
    }
}

file static class CompositionRegistryPolicy
{
    private static readonly List<ICompositionPolicy> _policy =
    [
        new PerfectSquarePolicy(), new NonZeroResultPolicy()
    ];

    public static bool IsValid(Composition composition)
    {
        return _policy.All(restriction => restriction.IsSatisfied(composition));
    }
}

file sealed class PerfectSquarePolicy : ICompositionPolicy
{
    public bool IsSatisfied(Composition composition)
    {
        return composition.OperatorType is GlyphType.SquareRoot && composition is BinaryComposition binary && binary.Lhs % binary.Rhs == 0;
    }
}

file sealed class NonZeroResultPolicy : ICompositionPolicy
{
    public bool IsSatisfied(Composition composition)
    {
        return composition.Result != 0;
    }
}

file interface ICompositionPolicy
{
    bool IsSatisfied(Composition composition);
}