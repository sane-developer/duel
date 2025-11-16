using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Policies;

namespace Duel.Modules.Engine.Muffs.Presets;

public interface IExpressionPreset
{
    public IDepthPolicy Depth { get; }
    
    public ILengthPolicy Length { get; }
    
    public IOperandPolicy Operand { get; }

    public IOperatorPolicy Operator { get; }

    public NumberRegistry NumberRegistry { get; }

    public CompositionRegistry CompositionRegistry { get; }
}