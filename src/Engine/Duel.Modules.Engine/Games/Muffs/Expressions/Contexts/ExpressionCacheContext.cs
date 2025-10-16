using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;
using Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Contexts;

public sealed class ExpressionCacheContext(DivisorVault divisors, ConstantVault constants, CompositionVault compositions)
{
    public Expression.Composition GetComposition(Random rng, int result)
    {
        return compositions.Get(rng, result);
    }

    public Constant GetConstant(int value)
    {
        return constants.Get(value);
    }

    public int GetDivisor(Random rng, int number)
    {
        return divisors.Get(rng, number);
    }
}