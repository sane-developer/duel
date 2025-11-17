using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Expressions.Parameters;

namespace Duel.Modules.Engine.Muffs.Expressions.Presets.Modes;

public sealed class EasyExpressionPreset : IExpressionPreset
{
    public IDepthParameter Depth => EasyExpressionPresetConfiguration.Depth;
    
    public ILengthParameter Length => EasyExpressionPresetConfiguration.Length;
    
    public IOperandParameter Operand => EasyExpressionPresetConfiguration.Operand;

    public IOperatorParameter Operator => EasyExpressionPresetConfiguration.Operator;

    public NumberRegistry NumberRegistry => EasyExpressionPresetConfiguration.NumberRegistry;

    public CompositionRegistry CompositionRegistry => EasyExpressionPresetConfiguration.CompositionRegistry;
}

file static class EasyExpressionPresetConfiguration
{
    public static readonly IDepthParameter Depth = new BoundedDepthParameter(0..2);

    public static readonly ILengthParameter Length = new BoundedLengthParameter(2..3);

    public static readonly IOperandParameter Operand = new BoundedOperandParameter(1, 10);

    public static readonly IOperatorParameter Operator = new WeightedOperatorParameter(_weights);

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