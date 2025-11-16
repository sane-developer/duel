using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Policies;

namespace Duel.Modules.Engine.Muffs.Presets.Modes;

public sealed class MediumExpressionPreset : IExpressionPreset
{
    public IDepthPolicy Depth => MediumExpressionPresetConfiguration.Depth;
    
    public ILengthPolicy Length => MediumExpressionPresetConfiguration.Length;
    
    public IOperandPolicy Operand => MediumExpressionPresetConfiguration.Operand;

    public IOperatorPolicy Operator => MediumExpressionPresetConfiguration.Operator;

    public NumberRegistry NumberRegistry => MediumExpressionPresetConfiguration.NumberRegistry;

    public CompositionRegistry CompositionRegistry => MediumExpressionPresetConfiguration.CompositionRegistry;
}

file static class MediumExpressionPresetConfiguration
{
    public static readonly IDepthPolicy Depth = new LimitedDepthPolicy(1..3);

    public static readonly ILengthPolicy Length = new LimitedLengthPolicy(2..4);

    public static readonly IOperandPolicy Operand = new LimitedOperandPolicy(-10, 10);

    public static readonly IOperatorPolicy Operator = new WeightedOperatorPolicy(_weights);

    public static readonly NumberRegistry NumberRegistry = new NumberRegistry(-200, 200);

    public static readonly CompositionRegistry CompositionRegistry = new CompositionRegistryBuilder()
        .WithAdditions(minimum: -10, maximum: 10)
        .WithSubtractions(minimum: -10, maximum: 10)
        .WithMultiplications(minimum: -10, maximum: 10)
        .WithDivisions(minimum: -10, maximum: 10)
        .WithModulos(minimum: -10, maximum: 10)
        .Build();

    private static readonly Dictionary<GlyphType, float> _weights = new()
    {
        { GlyphType.Add, 1.5f },
        { GlyphType.Subtract, 1.5f },
        { GlyphType.Multiply, 1.0f },
        { GlyphType.Divide, 0.75f },
        { GlyphType.Modulo, 0.25f },
    };
}