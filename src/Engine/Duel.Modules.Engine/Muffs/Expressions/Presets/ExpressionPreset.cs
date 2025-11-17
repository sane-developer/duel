using Duel.Modules.Engine.Muffs.Compositions;
using Duel.Modules.Engine.Muffs.Expressions.Parameters;

namespace Duel.Modules.Engine.Muffs.Expressions.Presets;

public interface IExpressionPreset
{
    public IDepthParameter Depth { get; }
    
    public ILengthParameter Length { get; }
    
    public IOperandParameter Operand { get; }

    public IOperatorParameter Operator { get; }

    public NumberRegistry NumberRegistry { get; }

    public CompositionRegistry CompositionRegistry { get; }
}