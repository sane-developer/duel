using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Filters;

public sealed class PerfectDivisorFilter : ICompositionFilter
{
    public bool Predicate(Composition composition)
    {
        return composition.Type is Expression.Operator.Divide && composition.Left % composition.Right is 0;
    }
}