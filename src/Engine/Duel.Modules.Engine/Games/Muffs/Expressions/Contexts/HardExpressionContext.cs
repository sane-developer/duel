using Duel.Shared.Ranges;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public sealed class HardExpressionContext(Random rng) : IExpressionContext
{
    public Random Rng => rng;

    public Range<int> Depth => ExpressionSettings.Depth;

    public Range<int> Constant => ExpressionSettings.Constant;

    public Range<int> Exponent => ExpressionSettings.Exponent;

    public Range<int> Operators => ExpressionSettings.Operations;

    public ExpressionVault Vault => ExpressionSettings.Vault;
}

file sealed class ExpressionSettings
{
    public static readonly Range<int> Depth = new(1, 10);
    
    public static readonly Range<int> Constant = new(1, 100);
    
    public static readonly Range<int> Exponent = new(1, 10);
    
    public static readonly Range<int> Operators = new(1, 10);

    public static readonly ExpressionVault Vault = ExpressionVault.Create(Constant.Minimum, Constant.Maximum);
}