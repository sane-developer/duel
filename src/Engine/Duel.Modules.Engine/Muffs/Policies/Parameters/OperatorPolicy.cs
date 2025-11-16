using Duel.Modules.Engine.Muffs.Glyphs;
using System.Collections.Frozen;

namespace Duel.Modules.Engine.Muffs.Generators.Policies;

public sealed class WeightedOperatorPolicy(Dictionary<GlyphType, float> weights) : IOperatorPolicy
{
    private readonly FrozenDictionary<GlyphType, float> _allOperators = weights.ToFrozenDictionary();

    private readonly FrozenDictionary<GlyphType, float> _binaryOperators = OperatorsFilter.Apply(weights).ToFrozenDictionary();

    public GlyphType GetAny(Random rng)
    {
        var remaining = rng.NextDouble() * _allOperators.Values.Sum();
        
        foreach (var (glyph, weight) in _allOperators)
        {
            if (remaining < weight)
            {
                return glyph;
            }

            remaining -= weight;
        }
        
        return Situation.Unreachable<GlyphType>();
    }

    public GlyphType GetBinary(Random rng)
    {
        var remaining = rng.NextDouble() * _binaryOperators.Values.Sum();
        
        foreach (var (glyph, weight) in _binaryOperators)
        {
            if (remaining < weight)
            {
                return glyph;
            }

            remaining -= weight;
        }

        return Situation.Unreachable<GlyphType>();
    }
}

file static class OperatorsFilter
{
    public static IDictionary<GlyphType, float> Apply(IDictionary<GlyphType, float> operators)
    {
        return operators.Where(IsBinary).ToDictionary(w => w.Key, w => w.Value);
    }

    private static bool IsBinary(KeyValuePair<GlyphType, float> metadata)
    {
        return metadata.Key 
            is GlyphType.Add 
            or GlyphType.Subtract 
            or GlyphType.Multiply 
            or GlyphType.Divide 
            or GlyphType.Modulo
            or GlyphType.Power;
    }
}