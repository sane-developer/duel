using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Expressions.Parameters;

namespace Duel.Modules.Engine.Muffs.Expressions.Presets.Modes;

public sealed class HardExpressionPreset : IExpressionPreset
{
    public IDepthParameter Depth => HardExpressionPresetConfiguration.Depth;
    
    public ILengthParameter Length => HardExpressionPresetConfiguration.Length;
    
    public IOperandParameter Operand => HardExpressionPresetConfiguration.Operand;

    public IOperatorParameter Operator => HardExpressionPresetConfiguration.Operator;

    public NumberRegistry NumberRegistry => HardExpressionPresetConfiguration.NumberRegistry;

    public CompositionRegistry CompositionRegistry => HardExpressionPresetConfiguration.CompositionRegistry;
}

file static class HardExpressionPresetConfiguration
{
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

    public static readonly IDepthParameter Depth = new BoundedDepthParameter(2..5);

    public static readonly ILengthParameter Length = new BoundedLengthParameter(3..6);

    public static readonly IOperandParameter Operand = new BoundedOperandParameter(-20, 20);

    public static readonly IOperatorParameter Operator = new WeightedOperatorParameter(_weights);

    public static readonly NumberRegistry NumberRegistry = new NumberRegistry(-500, 500);

    public static readonly CompositionRegistry CompositionRegistry = new CompositionRegistryBuilder()
        .WithAdditions(minimum: -20, maximum: 20)
        .WithSubtractions(minimum: -20, maximum: 20)
        .WithMultiplications(minimum: -20, maximum: 20)
        .WithDivisions(minimum: -20, maximum: 20)
        .WithModulos(minimum: -20, maximum: 20)
        .WithPowers(baseMinimum: -10, baseMaximum: 10, exponentMinimum: 0, exponentMaximum: 4)
        .WithNegations(minimum: -20, maximum: 20)
        .WithAbsoluteValues(minimum: -20, maximum: 20)
        .WithSquareRoots(minimum: 0, maximum: 20)
        .WithFactorials(minimum: 0, maximum: 7)
        .Build();
}