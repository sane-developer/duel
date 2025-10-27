using Duel.Modules.Engine.Games.Muffs.Generation;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Games.Muffs.Knowledge;

/// <summary>
/// Registry of all valid compositions, indexed by their result value.
/// Builds compositions from generator settings for fast target-based lookup.
/// </summary>
public sealed class CompositionsRegistry
{
    private readonly FrozenDictionary<int, Composition[]> _compositions;

    public CompositionsRegistry(GeneratorSettings settings)
    {
        var compositions = new List<Composition>();

        compositions.AddRange(CompositionBuilder.FromAddition(settings.Addition.Result.Start, settings.Addition.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromSubtraction(settings.Subtraction.Result.Start, settings.Subtraction.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromMultiplication(settings.Multiplication.Result.Start, settings.Multiplication.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromDivision(settings.Division.Result.Start, settings.Division.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromModulo(settings.Modulo.Result.Start, settings.Modulo.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromPower(settings.Power.Result.Start, settings.Power.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromNegation(settings.Negation.Result.Start, settings.Negation.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromAbsolute(settings.AbsoluteValue.Result.Start, settings.AbsoluteValue.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromFactorial(settings.Factorial.Result.Start, settings.Factorial.Result.End));
        
        compositions.AddRange(CompositionBuilder.FromSquareRoot(settings.SquareRoot.Result.Start, settings.SquareRoot.Result.End));

        _compositions = compositions
            .GroupBy(c => c.Result)
            .ToDictionary(g => g.Key, g => g.ToArray())
            .ToFrozenDictionary();
    }

    /// <summary>
    /// Gets all available result values that have at least one composition.
    /// </summary>
    public IReadOnlyCollection<int> AvailableResults => _compositions.Keys;

    /// <summary>
    /// Gets all compositions that produce the given result value.
    /// Returns an empty array if no compositions exist for the value.
    /// </summary>
    public Composition[] GetCompositions(int value)
    {
        return _compositions.GetValueOrDefault(value, []);
    }

    /// <summary>
    /// Gets all unique numbers referenced by compositions (operands and results).
    /// Used to populate the NumberCache.
    /// </summary>
    public IEnumerable<int> GetReferencedNumbers()
    {
        foreach (var compositions in _compositions.Values)
        {
            foreach (var composition in compositions)
            {
                yield return composition.Result;

                if (composition is BinaryComposition binary)
                {
                    yield return binary.Lhs;
                    
                    yield return binary.Rhs;
                }
                
                if (composition is UnaryComposition unary)
                {
                    yield return unary.Operand;
                }
            }
        }
    }
}

