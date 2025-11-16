using Duel.Modules.Engine.Muffs.Policies.Parameters;
using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Compositions;

namespace Duel.Modules.Engine.Muffs.Policies.Presets;

public sealed class MediumExpressionPreset : IExpressionPreset
{
    public IDepthPolicy Depth => MediumExpressionPresetConfiguration.Depth;
    
    public ILengthPolicy Length => MediumExpressionPresetConfiguration.Length;
    
    public IOperandPolicy Operand => MediumExpressionPresetConfiguration.Operand;

    public IOperatorPolicy Operator => MediumExpressionPresetConfiguration.Operator;

    public CompositionRegistry CompositionRegistry => MediumExpressionPresetConfiguration.CompositionRegistry;
}

file static class MediumExpressionPresetConfiguration
{
    public static readonly IDepthPolicy Depth = new LimitedDepthPolicy(1..4);

    public static readonly ILengthPolicy Length = new LimitedLengthPolicy(1..4);

    public static readonly IOperandPolicy Operand = new LimitedOperandPolicy(-10, 10);

    public static readonly IOperatorPolicy Operator = new WeightedOperatorPolicy(_weights);

    public static readonly CompositionRegistry CompositionRegistry = new CompositionRegistryBuilder()
        .WithAdditions(-10, 10)
        .WithSubtractions(-10, 10)
        .WithMultiplications(-10, 10)
        .WithDivisions(-10, 10)
        .WithModulos(-10, 10)
        .WithPowers(-10, 10)
        .Build();

    private static readonly Dictionary<GlyphType, float> _weights = new()
    {
        { GlyphType.Add, 1.0f },
        { GlyphType.Subtract, 1.0f },
        { GlyphType.Multiply, 1.0f },
        { GlyphType.Divide, 1.0f },
        { GlyphType.Modulo, 1.0f },
        { GlyphType.Power, 1.0f },
    };
}