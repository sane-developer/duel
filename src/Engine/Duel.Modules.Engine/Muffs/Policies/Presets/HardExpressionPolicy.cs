using Duel.Modules.Engine.Muffs.Policies.Parameters;
using Duel.Modules.Engine.Muffs.Glyphs;

namespace Duel.Modules.Engine.Muffs.Policies.Presets;

public sealed class HardExpressionPolicy : IExpressionPolicy
{
    public IDepthPolicy Depth => HardExpressionPolicyConfiguration.Depth;
    
    public ILengthPolicy Length => HardExpressionPolicyConfiguration.Length;
    
    public IOperandPolicy Operand => HardExpressionPolicyConfiguration.Operand;

    public IOperatorPolicy Operator => HardExpressionPolicyConfiguration.Operator;
}

file static class HardExpressionPolicyConfiguration
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
    });
}