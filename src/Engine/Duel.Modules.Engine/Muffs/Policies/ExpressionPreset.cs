using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Policies.Parameters;

namespace Duel.Modules.Engine.Muffs.Policies;

public interface IExpressionPreset
{
    public IDepthPolicy Depth { get; }
    
    public ILengthPolicy Length { get; }
    
    public IOperandPolicy Operand { get; }

    public IOperatorPolicy Operator { get; }

    public CompositionRegistry CompositionRegistry { get; }
}