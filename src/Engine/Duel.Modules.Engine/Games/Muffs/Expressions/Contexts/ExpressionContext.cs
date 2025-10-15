using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;
using Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public sealed class ExpressionContext(Random rng, ExpressionSettings settings)
{
    public Random Rng => rng;

    public Range Depth => settings.Depth;

    public Range Constant => settings.Constant;

    public Range Exponent => settings.Exponent;

    public Range Budget => settings.Budget;

    public Expression.Type[] Operations => settings.Operations;

    public readonly DivisorVault Divisors = DivisorVault.For(settings.Constant);

    public readonly CompositionVault Compositions = CompositionVaultFactory.Create(settings.Constant, settings.Operations);
}
