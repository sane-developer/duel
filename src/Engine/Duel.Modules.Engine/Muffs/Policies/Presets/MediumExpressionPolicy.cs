using Duel.Modules.Engine.Muffs.Policies.Parameters;
using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Policies.Presets;

public sealed class MediumExpressionPolicy : IExpressionPolicy
{
    public IDepthPolicy Depth => MediumExpressionPolicyConfiguration.Depth;
    
    public ILengthPolicy Length => MediumExpressionPolicyConfiguration.Length;
    
    public IOperandPolicy Operand => MediumExpressionPolicyConfiguration.Operand;

    public IOperatorPolicy Operator => MediumExpressionPolicyConfiguration.Operator;
}

file static class MediumExpressionPolicyConfiguration
{
    public static readonly IDepthPolicy Depth = new LimitedDepthPolicy(1..4);

    public static readonly ILengthPolicy Length = new LimitedLengthPolicy(1..4);

    public static readonly IOperandPolicy Operand = new LimitedOperandPolicy(-10, 10);

    public static readonly IOperatorPolicy Operator = new WeightedOperatorPolicy(new Dictionary<GlyphType, float>
    {
        { GlyphType.Add, 1.0f },
        { GlyphType.Subtract, 1.0f },
        { GlyphType.Multiply, 1.0f },
        { GlyphType.Divide, 1.0f },
        { GlyphType.Modulo, 1.0f },
        { GlyphType.Power, 1.0f },
        { GlyphType.Negate, 1.0f },
        { GlyphType.Absolute, 1.0f },
        { GlyphType.SquareRoot, 1.0f },
        { GlyphType.Factorial, 1.0f }
    });
}