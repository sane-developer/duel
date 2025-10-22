using Duel.Modules.Engine.Games.Muffs.Expressions.Symbols;

namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Filters;

public sealed class ExcludeTypeFilter(Expression.Operator type) : ICompositionFilter
{
    public bool Predicate(Composition composition)
    {
        return composition.Type != type;
    }
}