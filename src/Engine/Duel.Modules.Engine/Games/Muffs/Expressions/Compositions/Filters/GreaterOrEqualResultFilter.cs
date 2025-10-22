namespace Duel.Modules.Engine.Games.Muffs.Expressions.Compositions.Filters;

public sealed class GreaterOrEqualResultFilter(int value) : ICompositionFilter
{
    public bool Predicate(Composition composition)
    {
        return composition.Result >= value;
    }
}