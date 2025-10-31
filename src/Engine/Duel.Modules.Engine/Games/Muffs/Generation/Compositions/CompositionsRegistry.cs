using Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Compositions;

public sealed class CompositionsRegistry
{
    private readonly FrozenDictionary<int, Composition[]> _compositions;

    public CompositionsRegistry(Difficulty difficulty)
    {
        var compositions = new List<Composition>();

        foreach (var (type, settings) in difficulty.Operators)
        {
            compositions.AddRange(CompositionBuilder.From(type, settings.Result.Minimum, settings.Result.Maximum));
        }

        _compositions = compositions
            .GroupBy(c => c.Result)
            .ToDictionary(g => g.Key, g => g.ToArray())
            .ToFrozenDictionary();
    }

    public Composition[] GetCompositions(int value)
    {
        return _compositions.GetValueOrDefault(value, []);
    }
}