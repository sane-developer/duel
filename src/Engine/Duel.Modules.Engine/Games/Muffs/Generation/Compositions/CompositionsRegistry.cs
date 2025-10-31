using Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;
using Duel.Modules.Engine.Games.Muffs.Generation.Operators;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Compositions;

public sealed class CompositionsRegistry(Difficulty difficulty)
{
    private readonly FrozenDictionary<int, FrozenDictionary<OperatorType, Composition[]>> _compositions = Compile(difficulty).ToFrozenDictionary();

    public Composition[] GetCompositions(int result, OperatorType type)
    {
        if (!_compositions.TryGetValue(result, out var operators))
        {
            return [];
        }

        return operators.GetValueOrDefault(type, []);
    }

    private static Dictionary<int, FrozenDictionary<OperatorType, Composition[]>> Compile(Difficulty difficulty)
    {
        var results = new Dictionary<int, Dictionary<OperatorType, List<Composition>>>();

        foreach (var (type, settings) in difficulty.Operators)
        {
            var compositions = CompositionBuilder.From(type, settings.Result.Minimum, settings.Result.Maximum);

            foreach (var composition in compositions)
            {
                if (!results.TryGetValue(composition.Result, out var operators))
                {
                    operators = [];

                    results[composition.Result] = operators;
                }

                if (!operators.TryGetValue(composition.Type, out var expressions))
                {
                    expressions = [];

                    operators[composition.Type] = expressions;
                }

                expressions.Add(composition);
            }
        }

        return results.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ToDictionary(
                op => op.Key,
                op => op.Value.ToArray()
            ).ToFrozenDictionary()
        );
    }
}