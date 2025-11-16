using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Policies;

namespace Duel.Modules.Engine.Muffs.Presets.Modes;

public sealed class HardExpressionPreset : IExpressionPreset
{
    public IDepthPolicy Depth => HardExpressionPresetConfiguration.Depth;
    
    public ILengthPolicy Length => HardExpressionPresetConfiguration.Length;
    
    public IOperandPolicy Operand => HardExpressionPresetConfiguration.Operand;

    public IOperatorPolicy Operator => HardExpressionPresetConfiguration.Operator;

    public NumberRegistry NumberRegistry => HardExpressionPresetConfiguration.NumberRegistry;

    public CompositionRegistry CompositionRegistry => HardExpressionPresetConfiguration.CompositionRegistry;
}

file static class HardExpressionPresetConfiguration
{
    public static readonly IDepthPolicy Depth = new LimitedDepthPolicy(2..5);

    public static readonly ILengthPolicy Length = new LimitedLengthPolicy(3..6);

    public static readonly IOperandPolicy Operand = new LimitedOperandPolicy(-20, 20);

    public static readonly IOperatorPolicy Operator = new WeightedOperatorPolicy(_weights);

    public static readonly NumberRegistry NumberRegistry = new NumberRegistry(-500, 500);

    public static readonly CompositionRegistry CompositionRegistry = new CompositionRegistryBuilder()
        .WithAdditions(-20, 20)
        .WithSubtractions(-20, 20)
        .WithMultiplications(-20, 20)
        .WithDivisions(-20, 20)
        .WithModulos(-20, 20)
        .WithPowers(-10, 10, 0, 4)
        .WithNegations(-20, 20)
        .WithAbsoluteValues(-20, 20)
        .WithSquareRoots(0, 20)
        .WithFactorials(0, 7)
        .Build();

    private static readonly Dictionary<GlyphType, float> _weights = new()
    {
        { GlyphType.Add, 1.0f },
        { GlyphType.Subtract, 1.0f },
        { GlyphType.Multiply, 1.0f },
        { GlyphType.Divide, 0.75f },
        { GlyphType.Modulo, 0.5f },
        { GlyphType.Power, 0.25f },
        { GlyphType.Negate, 0.5f },
        { GlyphType.Absolute, 0.5f },
        { GlyphType.SquareRoot, 0.25f },
        { GlyphType.Factorial, 0.1f },
    };
}