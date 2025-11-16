using Duel.Modules.Engine.Muffs.Glyphs;
using Duel.Modules.Engine.Muffs.Generators.Policies;

namespace Duel.Modules.Engine.Muffs.Generators;

public interface IExpressionPolicy
{
    public IDepthPolicy Depth { get; }
    
    public ILengthPolicy Length { get; }
    
    public IOperandPolicy Operand { get; }

    public IOperatorPolicy Operator { get; }
}

public sealed class EasyExpressionPolicy : IExpressionPolicy
{
    public IDepthPolicy Depth { get; } = new LimitedDepthPolicy(1..4);
    
    public ILengthPolicy Length { get; } = new LimitedLengthPolicy(1..4);
    
    public IOperandPolicy Operand { get; } = new LimitedOperandPolicy(-10, 10);

    public IOperatorPolicy Operator { get; } = new WeightedOperatorPolicy(new Dictionary<GlyphType, float>
    {
        { GlyphType.Add, 1.0f },
        { GlyphType.Subtract, 1.0f },
        { GlyphType.Multiply, 1.0f },
        { GlyphType.Divide, 1.0f },
        { GlyphType.Modulo, 1.0f },
        { GlyphType.Power, 1.0f },
    });
}

public sealed class MediumExpressionPolicy : IExpressionPolicy
{
    public IDepthPolicy Depth { get; } = new LimitedDepthPolicy(1..6);
    
    public ILengthPolicy Length { get; } = new LimitedLengthPolicy(1..6);
    
    public IOperandPolicy Operand { get; } = new LimitedOperandPolicy(-20, 20);
    
    public IOperatorPolicy Operator { get; } = new WeightedOperatorPolicy(new Dictionary<GlyphType, float>
    {
        { GlyphType.Add, 1.0f },
        { GlyphType.Subtract, 1.0f },
        { GlyphType.Multiply, 1.0f },
        { GlyphType.Divide, 1.0f },
        { GlyphType.Modulo, 1.0f },
        { GlyphType.Power, 1.0f },
    });
}

public sealed class HardExpressionPolicy : IExpressionPolicy
{
    public IDepthPolicy Depth { get; } = new LimitedDepthPolicy(1..8);
    
    public ILengthPolicy Length { get; } = new LimitedLengthPolicy(1..8);
    
    public IOperandPolicy Operand { get; } = new LimitedOperandPolicy(-30, 30);

    public IOperatorPolicy Operator { get; } = new WeightedOperatorPolicy(new Dictionary<GlyphType, float>
    {
        { GlyphType.Add, 1.0f },
        { GlyphType.Subtract, 1.0f },
        { GlyphType.Multiply, 1.0f },
        { GlyphType.Divide, 1.0f },
        { GlyphType.Modulo, 1.0f },
        { GlyphType.Power, 1.0f },
    });
}

public interface IDepthPolicy
{
    int GetDepth(Random rng);
}

public interface ILengthPolicy
{
    int GetLength(Random rng);
}

public interface IOperatorPolicy
{
    GlyphType GetAny(Random rng);

    GlyphType GetBinary(Random rng);
}

public interface IOperandPolicy
{
    int GetNumber(Random rng);
}