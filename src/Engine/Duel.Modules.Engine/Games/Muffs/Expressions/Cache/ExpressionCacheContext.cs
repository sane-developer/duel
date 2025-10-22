using Duel.Modules.Engine.Games.Muffs.Expressions.Compositions;
using Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Filters;
using Duel.Modules.Engine.Games.Muffs.Expressions.Settings;
using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;
using Duel.Modules.Engine.Games.Muffs.Expressions.Vaults;
using Duel.Shared.Extensions;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Cache;

public sealed class ExpressionCacheContext(ExpressionSettings settings)
{
    private List<Composition> _set => CompositionSet.From(settings.Constant)
        .Apply(new GreaterOrEqualResultFilter(settings.Budget.Start.Value))
        .Apply(new SmallerOrEqualResultFilter(settings.Budget.End.Value))
        .Apply(new NonZeroResultFilter())
        .Apply(new PerfectDivisorFilter())
        .Build();

    private readonly CompositionsVault _compositions = CompositionsVault.From(_set);
    
    private readonly ConstantsVault _constants = ConstantsVault.From(settings.Constant);

    private readonly DivisorsVault _divisors = DivisorsVault.From(settings.Constant);

    public Constant GetConstant(int value)
    {
        return _constants.For(value);
    }

    public int GetDivisor(Random rng, int number)
    {
        return _divisors.For(number).Element(rng);
    }

    public Composition GetComposition(Random rng, int number)
    {
        return _compositions.For(number).Element(rng);
    }
}