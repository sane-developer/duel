using Duel.Shared.Ranges;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public sealed class MediumExpressionContext(Random rng) : IExpressionContext
{
    public Random Rng => rng;

    public Range<int> Depth => _depth;

    public Range<int> Constant => _constant;

    public Range<int> Exponent => _exponent;

    public Range<int> Operators => _operators;

    public ExpressionVault Vault => ExpressionVault.Create(rng, Constant.Minimum, Constant.Maximum);

    private static readonly Range<int> _depth = new(1, 10);
    
    private static readonly Range<int> _constant = new(1, 100);
    
    private static readonly Range<int> _exponent = new(1, 10);
    
    private static readonly Range<int> _operators = new(1, 10);
}