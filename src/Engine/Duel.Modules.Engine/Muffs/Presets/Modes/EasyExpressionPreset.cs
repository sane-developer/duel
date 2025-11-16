using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Policies;

namespace Duel.Modules.Engine.Muffs.Presets.Modes;

public sealed class EasyExpressionPreset : IExpressionPreset
{
    public IDepthPolicy Depth => EasyExpressionPresetConfiguration.Depth;
    
    public ILengthPolicy Length => EasyExpressionPresetConfiguration.Length;
    
    public IOperandPolicy Operand => EasyExpressionPresetConfiguration.Operand;

    public IOperatorPolicy Operator => EasyExpressionPresetConfiguration.Operator;

    public NumberRegistry NumberRegistry => EasyExpressionPresetConfiguration.NumberRegistry;

    public CompositionRegistry CompositionRegistry => EasyExpressionPresetConfiguration.CompositionRegistry;
}

file static class EasyExpressionPresetConfiguration
{
    public static readonly IDepthPolicy Depth = new LimitedDepthPolicy(0..2);

    public static readonly ILengthPolicy Length = new LimitedLengthPolicy(2..3);

    public static readonly IOperandPolicy Operand = new LimitedOperandPolicy(1, 10);

    public static readonly IOperatorPolicy Operator = new WeightedOperatorPolicy(_weights);

    public static readonly NumberRegistry NumberRegistry = new NumberRegistry(-50, 150);

    public static readonly CompositionRegistry CompositionRegistry = new CompositionRegistryBuilder()
        .WithAdditions(minimum: 1, maximum: 10)
        .WithSubtractions(minimum: 1, maximum: 10)
        .WithMultiplications(minimum: 1, maximum: 10)
        .Build();

    private static readonly Dictionary<GlyphType, float> _weights = new()
    {
        { GlyphType.Add, 3.0f },
        { GlyphType.Subtract, 1.5f },
        { GlyphType.Multiply, 1.5f },
    };
}