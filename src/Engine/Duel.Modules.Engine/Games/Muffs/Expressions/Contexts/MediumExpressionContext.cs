using Duel.Shared.Ranges;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public sealed class MediumExpressionContext(Random rng) : IExpressionContext
{
    public Random Rng => rng;

    public Range<int> Depth => new(1, 10);

    public Range<int> Constant => new(1, 100);

    public Range<int> Exponent => new(1, 10);
    
    public Range<int> Operators => new(1, 10);
}