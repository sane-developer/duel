using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Expressions.Parameters;

namespace Duel.Modules.Engine.Muffs.Expressions.Presets.Modes;

public sealed class MediumExpressionPreset : IExpressionPreset
{
    public IDepthParameter Depth => MediumExpressionPresetConfiguration.Depth;
    
    public ILengthParameter Length => MediumExpressionPresetConfiguration.Length;
    
    public IOperandParameter Operand => MediumExpressionPresetConfiguration.Operand;

    public IOperatorParameter Operator => MediumExpressionPresetConfiguration.Operator;

    public NumberRegistry NumberRegistry => MediumExpressionPresetConfiguration.NumberRegistry;

    public CompositionRegistry CompositionRegistry => MediumExpressionPresetConfiguration.CompositionRegistry;
}

file static class MediumExpressionPresetConfiguration
{
    public static readonly IDepthParameter Depth = new BoundedDepthParameter(1..3);

    public static readonly ILengthParameter Length = new BoundedLengthParameter(2..4);

    public static readonly IOperandParameter Operand = new BoundedOperandParameter(-10, 10);

    public static readonly IOperatorParameter Operator = new WeightedOperatorParameter(_weights);

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