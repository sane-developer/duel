using Duel.Modules.Engine.Muffs.Glyphs;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Muffs.Compositions;

public sealed class CompositionRegistry(Dictionary<int, List<Composition>> registries)
{
    private readonly FrozenDictionary<int, List<Composition>> _registries = CompositionRegistryPolicy.Apply(registries);

    public List<Composition> For(int result)
    {
        return _registries[result];
    }
}

file static class CompositionRegistryPolicy
{
    private static readonly List<ICompositionPolicy> _policy = 
    [
        new PerfectSquarePolicy(),
        new NonZeroResultPolicy()
    ];

    public static FrozenDictionary<int, List<Composition>> Apply(IDictionary<int, List<Composition>> compositions)
    {
        return compositions.ToFrozenDictionary(x => x.Key, x => Filter(x.Value));
    }

    private static List<Composition> Filter(List<Composition> compositions)
    {
        return [.. compositions.Where(IsValid)];
    }

    private static bool IsValid(Composition composition)
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