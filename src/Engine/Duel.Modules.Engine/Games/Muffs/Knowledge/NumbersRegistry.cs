using Duel.Modules.Engine.Games.Muffs.Representation;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Knowledge;

/// <summary>
/// Caches Number literal instances for reuse across expression generation.
/// Ensures the same number value always returns the same Number instance.
/// </summary>
public sealed class NumbersRegistry(CompositionsRegistry registry)
{
    private readonly FrozenDictionary<int, Number> _numbers = registry
        .GetReferencedNumbers()
        .Distinct()
        .ToDictionary(n => n, n => new Number(n))
        .ToFrozenDictionary();

    /// <summary>
    /// Gets a cached Number instance for the given value.
    /// </summary>
    public Number GetNumber(int value)
    {
        return _numbers[value];
    }
}