using Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Compositions;

public sealed class CompositionsRegistry(Difficulty difficulty)
{
    private readonly FrozenDictionary<int, Composition[]> _compositions = Compile(difficulty).ToFrozenDictionary();

    public Composition[] GetCompositions(int value)
    {
        return _compositions.GetValueOrDefault(value, []);
    }

    private static Dictionary<int, Composition[]> Compile(Difficulty difficulty)
    {
        var results = new List<Composition>();

        foreach (var (type, settings) in difficulty.Operators)
        {
            var compositions = CompositionBuilder.From(type, settings.Result.Minimum, settings.Result.Maximum);

            results.AddRange(compositions);
        }

        return results.GroupBy(c => c.Result).ToDictionary(g => g.Key, g => g.ToArray());
    }
}