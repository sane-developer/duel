using Duel.Modules.Engine.Games.Muffs.Generation.Difficulties;
using Duel.Modules.Engine.Games.Muffs.Generation.Operators;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Generation.Compositions;

public sealed class CompositionsRegistry(Difficulty difficulty)
{
    private readonly FrozenDictionary<int, FrozenDictionary<OperatorType, Composition[]>> _compositions = Compile(difficulty);

    public Composition[] GetCompositions(int result, OperatorType type)
    {
        if (!_compositions.TryGetValue(result, out var operators))
        {
            return [];
        }

        return operators.GetValueOrDefault(type, []);
    }

    private static FrozenDictionary<int, FrozenDictionary<OperatorType, Composition[]>> Compile(Difficulty difficulty)
    {
        var results = new Dictionary<int, Dictionary<OperatorType, List<Composition>>>();

        foreach (var (type, settings) in difficulty.Operators)
        {
            var compositions = CompositionBuilder.For(type, settings.Result);

            foreach (var composition in compositions)
            {
                if (!results.TryGetValue(composition.Result, out var operators))
                {
                    operators = [];

                    results[composition.Result] = operators;
                }

                if (!operators.TryGetValue(composition.Type, out var typeCompositions))
                {
                    typeCompositions = [];

                    operators[composition.Type] = typeCompositions;
                }

                typeCompositions.Add(composition);
            }
        }

        return results.ToFrozenDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value
                .ToDictionary(op => op.Key, op => op.Value.ToArray())
                .ToFrozenDictionary()
        );
    }
}